using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace WaveHandler
{
  public partial class Form1 : Form
  {
    // Global members
    private event GeneralMessageEventHandler GeneralMessageEvent;
    WaveStructs.NewWaveInfo g_oMergedWaveInfo = new WaveStructs.NewWaveInfo();
    ProgressBar g_oProgressDialog             = new ProgressBar();
    string g_sLoadedProjectName;
    string g_sLoadedProjectPath;

    public Form1()
    {
      InitializeComponent();

      GeneralMessageEvent += new GeneralMessageEventHandler(this.OnShowMessageBox);

      // Set the defaults
      g_oMergedWaveInfo.dwSamplesPerSec  = 44100;
      g_oMergedWaveInfo.wBitsPerSample   = 16;
      g_oMergedWaveInfo.wChannels        = 2;
      g_oMergedWaveInfo.dwAvgBytesPerSec = Convert.ToUInt32(g_oMergedWaveInfo.dwSamplesPerSec * ((g_oMergedWaveInfo.wBitsPerSample * g_oMergedWaveInfo.wChannels) / 8));
      g_oMergedWaveInfo.wBlockAlign      = Convert.ToUInt16(g_oMergedWaveInfo.wChannels * g_oMergedWaveInfo.wBitsPerSample / 8);
      g_oMergedWaveInfo.wFormatTag       = 1; // Only non-compressed data allowed for now

      AvailableFilesListBox.DataSource = new ArrayList();
      ProjectFilesListBox.DataSource   = new ArrayList();
    }

    /// <summary>
    /// This method must be used when creating new wave file that should be capable of firing custom events.
    /// </summary>
    /// <returns></returns>
    private WaveFile CreateNewWaveFileObject()
    {
      WaveFile oWaveFile              = new WaveFile();
      oWaveFile.GeneralMessageEvent   += new GeneralMessageEventHandler(this.OnShowMessageBox);
      oWaveFile.ExtendedMessageEvent  += new ExtendedMessageEventHandler(this.OnShowMessageBoxExt);
      return oWaveFile;
    }

    private void ShowMessageBox(string sCaption, string sText)
    {
      this.GeneralMessageEvent(this, new WaveHandlerEventArgs(sCaption, sText));
    }

    private void OnShowMessageBox(object sender, WaveHandlerEventArgs e)
    {
      bool bProgressDialogWasOpen = g_oProgressDialog.Visible;

      if (bProgressDialogWasOpen)
      {
        g_oProgressDialog.Hide();
      }

      GeneralMessageBox oMessageBox = new GeneralMessageBox();
      oMessageBox.ShowInTaskbar     = false;
      oMessageBox.TopMost           = true;
      oMessageBox.Focus();
      oMessageBox.BringToFront();
      oMessageBox.TopMost           = false;
      oMessageBox.CenterDialogToParent();
      oMessageBox.SetCaption(e.sDialogCaption);
      oMessageBox.SetText(e.sEventText);
      oMessageBox.ShowDialog(this);

      this.Focus();
      this.BringToFront();

      if (bProgressDialogWasOpen)
      {
        g_oProgressDialog.Show(this);
      }
    }

    private DialogResult OnShowMessageBoxExt(object sender, WaveHandlerEventArgs e)
    {
      bool bProgressDialogWasOpen = g_oProgressDialog.Visible;

      if (bProgressDialogWasOpen)
      {
        g_oProgressDialog.Hide();
      }

      MessageBoxExt oMessageBoxExt = new MessageBoxExt();
      oMessageBoxExt.ShowInTaskbar = false;
      oMessageBoxExt.TopMost       = true;
      oMessageBoxExt.Focus();
      oMessageBoxExt.BringToFront();
      oMessageBoxExt.TopMost       = false;
      oMessageBoxExt.CenterDialogToParent();
      oMessageBoxExt.SetCaption(e.sDialogCaption);
      oMessageBoxExt.SetText(e.sEventText);
      oMessageBoxExt.SetOkButtonText(e.sDialogOkButtonText);
      oMessageBoxExt.SetCancelButtonText(e.sDialogCancelButtonText);
      DialogResult oResult = oMessageBoxExt.ShowDialog(this);

      this.Focus();
      this.BringToFront();

      if (bProgressDialogWasOpen)
      {
        g_oProgressDialog.Show(this);
      }

      return oResult;
    }

    private void ProjectFilesListBox_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        string[] aFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

        ArrayList aoListEntryInfos = (ArrayList)ProjectFilesListBox.DataSource;

        g_oProgressDialog.Text = "Adding Wave Files to the Project Files collection via DragnDrop";
        g_oProgressDialog.CenterDialogToParent();
        g_oProgressDialog.Show(this);
        string sText = String.Empty;
        int nCount = 0;

        foreach (string sFile in aFiles)
        {
          if (String.Equals(Path.GetExtension(sFile), ".wav", StringComparison.OrdinalIgnoreCase))
          {
            ExtendedListEntry.ExtendedListEntry oListEntryInfo = new ExtendedListEntry.ExtendedListEntry(Path.GetFileName(sFile), sFile);

            if (!aoListEntryInfos.Contains(oListEntryInfo))
            {
              aoListEntryInfos.Add(oListEntryInfo);
            }
          }

          sText = (++nCount).ToString();
          sText += "/";
          sText += aFiles.Length;
          g_oProgressDialog.SetText(sText);
          g_oProgressDialog.SetProgress(100.0 * Convert.ToDouble(nCount) / Convert.ToDouble(aFiles.Length));
          g_oProgressDialog.Update();
        }

        g_oProgressDialog.SetText("SORTING!");
        g_oProgressDialog.Update();
        aoListEntryInfos.Sort();
        ProjectFilesListBox.BeginUpdate();
        ProjectFilesListBox.SuspendLayout();
        ProjectFilesListBox.DataSource = null;
        ProjectFilesListBox.DisplayMember = "GetSetName";
        ProjectFilesListBox.ValueMember = "GetSetPath";
        ProjectFilesListBox.DataSource = aoListEntryInfos;
        ProjectFilesListBox.ResumeLayout();
        ProjectFilesListBox.EndUpdate();
        g_oProgressDialog.Hide();
      }
    }

    private void ProjectFilesListBox_DragOver(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        e.Effect = DragDropEffects.Move;
      }
    }

    private void button6_Click(object sender, EventArgs e)
    {
      folderBrowserDialog1.SelectedPath = Path.GetDirectoryName(Application.ExecutablePath);
      DialogResult result               = folderBrowserDialog1.ShowDialog();

      if (result == DialogResult.OK)
      {
        MessageBoxSimple oMessageBoxSimple = new MessageBoxSimple();
        oMessageBoxSimple.ShowInTaskbar    = false;
        oMessageBoxSimple.TopMost          = true;
        oMessageBoxSimple.Focus();
        oMessageBoxSimple.BringToFront();
        oMessageBoxSimple.SetCaption("Looking For Wave Files");
        oMessageBoxSimple.SetText("Please wait while a list of available wave files is compiled.");
        oMessageBoxSimple.Show(this);
        oMessageBoxSimple.CenterDialogToParent();
        oMessageBoxSimple.Update(); // So it's seen as the main form will block during file search.
        oMessageBoxSimple.TopMost = false;

        ArrayList aFiles = new ArrayList();

        // Do this once for the top folder.
        DirectoryInfo oDirInfo = new DirectoryInfo(folderBrowserDialog1.SelectedPath);

        try
        {
          aFiles.AddRange(oDirInfo.GetFiles("*.wav", SearchOption.TopDirectoryOnly));
        }
        catch (System.UnauthorizedAccessException)
        {
          // Ignore this directory and keep looking.
          //string sDesc = String.Format("You do not have access to directory: {0}", oInfo.Name);
          //ShowMessageBox("No Access To Directory", sDesc);
        }
        catch (System.IO.IOException)
        {
          // This might be worth a notification but keep looking.
          ShowMessageBox("Access Exception Occurred", "Looks like the drive is not valid!");

          oMessageBoxSimple.Close();
          return;
        }

        // Now get all the wave files in the valid sub folders if allowed.
        if (checkBox1.CheckState == CheckState.Checked)
        {
          DirectoryInfo[] aDirectories = oDirInfo.GetDirectories();

          foreach (DirectoryInfo oInfo in aDirectories)
          {
            try
            {
              aFiles.AddRange(oInfo.GetFiles("*.wav", SearchOption.AllDirectories));
            }
            catch (System.UnauthorizedAccessException)
            {
              // Ignore this directory and keep looking.
              //string sDesc = String.Format("You do not have access to directory: {0}", oInfo.Name);
              //ShowMessageBox("No Access To Directory", sDesc);
            }
            catch (System.IO.IOException)
            {
              // This might be worth a notification but keep looking.
              ShowMessageBox("Access Exception Occurred", "Looks like the drive is not valid!");
            }
          }
        }

        oMessageBoxSimple.Close();
        this.TopMost = true;
        this.Focus();
        this.BringToFront();
        this.TopMost = false;

        if (aFiles.Count == 0)
        {
          ShowMessageBox("No Wave Files Found", "Could not find any wave files at the location!");

          return;
        }

        ArrayList aoListEntryInfos = (ArrayList)AvailableFilesListBox.DataSource;

        g_oProgressDialog.Text = "Looking For Wave Files";
        g_oProgressDialog.CenterDialogToParent();
        g_oProgressDialog.Show(this);
        string sText = String.Empty;
        int nCount = 0;

        foreach (FileInfo oFileInfo in aFiles)
        {
          ExtendedListEntry.ExtendedListEntry oListEntryInfo = new ExtendedListEntry.ExtendedListEntry(oFileInfo.Name, oFileInfo.FullName);

          if (!aoListEntryInfos.Contains(oListEntryInfo))
          {
            aoListEntryInfos.Add(oListEntryInfo);
          }

          sText = (++nCount).ToString();
          sText += "/";
          sText += aFiles.Count;
          g_oProgressDialog.SetText(sText);
          g_oProgressDialog.SetProgress(100.0 * Convert.ToDouble(nCount) / Convert.ToDouble(aFiles.Count));
          g_oProgressDialog.Update();
        }

        g_oProgressDialog.SetText("SORTING!");
        g_oProgressDialog.Update();
        aoListEntryInfos.Sort();
        AvailableFilesListBox.BeginUpdate();
        AvailableFilesListBox.SuspendLayout();
        AvailableFilesListBox.DataSource = null;
        AvailableFilesListBox.DisplayMember = "GetSetName";
        AvailableFilesListBox.ValueMember = "GetSetPath";
        AvailableFilesListBox.DataSource = aoListEntryInfos;
        AvailableFilesListBox.ResumeLayout();
        AvailableFilesListBox.EndUpdate();
        g_oProgressDialog.Hide();
      }
    }

    private void button4_Click(object sender, EventArgs e)
    {
      // Move selected items in AvailableFilesListBox to ProjectFilesListBox
      ArrayList aoListEntryInfos = (ArrayList)ProjectFilesListBox.DataSource;

      g_oProgressDialog.Text = "Copying Wave Files To Project Files Collection";
      g_oProgressDialog.CenterDialogToParent();
      g_oProgressDialog.Show(this);
      string sText = String.Empty;
      int nCount = 0;
      bool bInvalidateProject = false;

      int[] aSelectedIndices = AvailableFilesListBox.GetSelectedIndices();

      for (int i = 0; i < aSelectedIndices.Length; ++i)
      {
        object oItem = AvailableFilesListBox.Items[aSelectedIndices[i]];

        if (!aoListEntryInfos.Contains(oItem))
        {
          ExtendedListEntry.ExtendedListEntry oEntry = oItem as ExtendedListEntry.ExtendedListEntry;
          aoListEntryInfos.Add(oEntry.Clone());

          // This as well invalidates a loaded project
          bInvalidateProject = true;
        }

        sText = (++nCount).ToString();
        sText += "/";
        sText += AvailableFilesListBox.SelectedItems.Count;
        g_oProgressDialog.SetText(sText);
        g_oProgressDialog.SetProgress(100.0 * Convert.ToDouble(nCount) / Convert.ToDouble(AvailableFilesListBox.SelectedItems.Count));
        g_oProgressDialog.Update();
      }

      if (bInvalidateProject == true)
      {
        InvalidateProject();
      }

      g_oProgressDialog.SetText("SORTING!");
      g_oProgressDialog.Update();
      aoListEntryInfos.Sort();
      ProjectFilesListBox.BeginUpdate();
      ProjectFilesListBox.SuspendLayout();
      ProjectFilesListBox.DataSource = null;
      ProjectFilesListBox.DisplayMember = "GetSetName";
      ProjectFilesListBox.ValueMember = "GetSetPath";
      ProjectFilesListBox.DataSource = aoListEntryInfos;
      ProjectFilesListBox.ResumeLayout();
      ProjectFilesListBox.EndUpdate();
      g_oProgressDialog.Hide();
    }

    private void button5_Click(object sender, EventArgs e)
    {
      // Remove selected items in ProjectFilesListBox from ProjectFilesListBox
      int[] aSelectedIndices = ProjectFilesListBox.GetSelectedIndices();

      if (aSelectedIndices.Length > 0)
      {
        ArrayList aoListEntryInfos = (ArrayList)ProjectFilesListBox.DataSource;

        // In case the user selected all entries make sure to just call a clear()
        if (aSelectedIndices.Length == aoListEntryInfos.Count)
        {
          aoListEntryInfos.Clear();
        }
        else
        {
          g_oProgressDialog.Text = "Removing Wave Files From Project Files Collection";
          g_oProgressDialog.CenterDialogToParent();
          g_oProgressDialog.Show(this);
          string sText = String.Empty;
          int nCount = 0;
          int nTopIndex = ProjectFilesListBox.TopIndex;

          for (int i = 0; i < aSelectedIndices.Length; ++i)
          {
            object oItem = ProjectFilesListBox.Items[aSelectedIndices[i]];

            aoListEntryInfos.Remove(oItem);

            sText = (++nCount).ToString();
            sText += "/";
            sText += ProjectFilesListBox.SelectedItems.Count;
            g_oProgressDialog.SetText(sText);
            g_oProgressDialog.SetProgress(100.0 * Convert.ToDouble(nCount) / Convert.ToDouble(ProjectFilesListBox.SelectedItems.Count));
            g_oProgressDialog.Update();
          }

          ProjectFilesListBox.TopIndex = (ProjectFilesListBox.Items.Count > nTopIndex) ? nTopIndex : ProjectFilesListBox.Items.Count;

          g_oProgressDialog.SetText("SORTING!");
          g_oProgressDialog.Update();
        }

        aoListEntryInfos.Sort();
        ProjectFilesListBox.BeginUpdate();
        ProjectFilesListBox.SuspendLayout();
        ProjectFilesListBox.DataSource = null;
        ProjectFilesListBox.DisplayMember = "GetSetName";
        ProjectFilesListBox.ValueMember = "GetSetPath";
        ProjectFilesListBox.DataSource = aoListEntryInfos;
        ProjectFilesListBox.ResumeLayout();
        ProjectFilesListBox.EndUpdate();
        g_oProgressDialog.Hide();

        // This as well invalidates a loaded project
        InvalidateProject();
      }
    }

    private void AvailableFilesListBox_KeyDown(object sender, KeyEventArgs e)
    {
      // Prevent the 'A' key from selecting a new entry starting with 'a'
      if (e.KeyCode == Keys.A && e.Control)
      {
        e.SuppressKeyPress = true;
        AvailableFilesListBox.SelectAll();
      }
    }

    private void ProjectFilesListBox_KeyDown(object sender, KeyEventArgs e)
    {
      // Prevent the 'A' key from selecting a new entry starting with 'a'
      if (e.KeyCode == Keys.A && e.Control)
      {
        e.SuppressKeyPress = true;
        ProjectFilesListBox.SelectAll();
      }
    }

    /// <summary>
    /// Display a tooltip for entries in list box #1
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AvailableFilesListBox_MouseHover(object sender, EventArgs e)
    {
      // Display a tooltip for the item we're hovering over to see its full path
      Point oCursorPosition = Cursor.Position;

      // Get the last item's bottom position.
      oCursorPosition = AvailableFilesListBox.PointToClient(oCursorPosition);
      int nItemIndex = AvailableFilesListBox.IndexFromPoint(oCursorPosition);

      if (nItemIndex >= 0 && nItemIndex < 65535)
      {
        Rectangle oRectangle = AvailableFilesListBox.GetItemRectangle(AvailableFilesListBox.Items.Count - 1);

        if (oCursorPosition.Y < oRectangle.Bottom)
        {
          ExtendedListEntry.ExtendedListEntry oListEntryInfo = ((ExtendedListEntry.ExtendedListEntry)AvailableFilesListBox.Items[nItemIndex]);
          ShowToolTip(oListEntryInfo, AvailableFilesListBox);
        }
      }
    }

    /// <summary>
    /// Display a tooltip for entries in list box #2
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProjectFilesListBox_MouseHover(object sender, EventArgs e)
    {
      // Display a tooltip for the item we're hovering over to see its full path
      Point oCursorPosition = Cursor.Position;
      oCursorPosition = ProjectFilesListBox.PointToClient(oCursorPosition);
      int nItemIndex = ProjectFilesListBox.IndexFromPoint(oCursorPosition);

      if (nItemIndex >= 0 && nItemIndex < 65535)
      {
        ExtendedListEntry.ExtendedListEntry oListEntryInfo = ((ExtendedListEntry.ExtendedListEntry)ProjectFilesListBox.Items[nItemIndex]);
        ShowToolTip(oListEntryInfo, ProjectFilesListBox);
      }
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
      if (AvailableFilesListBox.Items.Count > 0)
      {
        AvailableFilesListBox.ExtDisplayMember = (checkBox2.CheckState == CheckState.Checked) ? "GetSetPath" : "GetSetName";
        AvailableFilesListBox.ExtValueMember = (checkBox2.CheckState == CheckState.Checked) ? "GetSetName" : "GetSetPath";
      }

      if (ProjectFilesListBox.Items.Count > 0)
      {
        ProjectFilesListBox.ExtDisplayMember = (checkBox2.CheckState == CheckState.Checked) ? "GetSetPath" : "GetSetName";
        ProjectFilesListBox.ExtValueMember = (checkBox2.CheckState == CheckState.Checked) ? "GetSetName" : "GetSetPath";
      }
    }

    private void CreateXML(string sProjectPath)
    {
      string sProjectName          = Path.GetFileName(sProjectPath);
      XmlTextWriter oXmlTextWriter = new XmlTextWriter(sProjectPath + "\\" + sProjectName + ".xml", Encoding.Default);
      oXmlTextWriter.Formatting    = Formatting.Indented;
      oXmlTextWriter.WriteStartDocument(false);
      oXmlTextWriter.WriteStartElement("Projects");
      oXmlTextWriter.WriteStartElement("Project");
      oXmlTextWriter.WriteAttributeString("name", sProjectName);
      oXmlTextWriter.WriteAttributeString("format", g_oMergedWaveInfo.wFormatTag.ToString());
      oXmlTextWriter.WriteAttributeString("channels", g_oMergedWaveInfo.wChannels.ToString());
      oXmlTextWriter.WriteAttributeString("samplespersec", g_oMergedWaveInfo.dwSamplesPerSec.ToString());
      oXmlTextWriter.WriteAttributeString("avgbytespersec", g_oMergedWaveInfo.dwAvgBytesPerSec.ToString());
      oXmlTextWriter.WriteAttributeString("blockalign", g_oMergedWaveInfo.wBlockAlign.ToString());
      oXmlTextWriter.WriteAttributeString("bitspersample", g_oMergedWaveInfo.wBitsPerSample.ToString());
      oXmlTextWriter.WriteAttributeString("filescount", ProjectFilesListBox.Items.Count.ToString());

      // Make sure to reset this one again before calculating the merged file data size
      g_oMergedWaveInfo.dwDataSize = 0;

      foreach (object oItem in ProjectFilesListBox.Items)
      {
        oXmlTextWriter.WriteStartElement("WaveFile");
        ExtendedListEntry.ExtendedListEntry oListEntryInfo = (ExtendedListEntry.ExtendedListEntry)oItem;
        oXmlTextWriter.WriteAttributeString("name", oListEntryInfo.GetSetName);
        oXmlTextWriter.WriteAttributeString("path", oListEntryInfo.GetSetPath);

        WaveFile oWaveFile = CreateNewWaveFileObject();

        if (oWaveFile.Initialize(oListEntryInfo.GetSetPath, false))
        {
          // Data chunk info.
          WaveStructs.DataChunk oDataChunk = oWaveFile.GetDataChunk();
          oXmlTextWriter.WriteAttributeString("datasize", oDataChunk.dwSize.ToString());
          g_oMergedWaveInfo.dwDataSize += oDataChunk.dwSize;

          // Fact chunk info.
          WaveStructs.FactChunk oFactChunk = oWaveFile.GetFactChunk();

          if (oFactChunk.dwSize > 0)
          {
            oXmlTextWriter.WriteStartElement("FactChunk");
            oXmlTextWriter.WriteAttributeString("size", oFactChunk.dwSize.ToString());
            oXmlTextWriter.WriteAttributeString("numsamples", oFactChunk.dwNumSamples.ToString());
            oXmlTextWriter.WriteEndElement(); // FactChunk.
          }

          // Cue chunk info.
          WaveStructs.CueChunk oCueChunk = oWaveFile.GetCueChunk();

          if (oCueChunk.dwSize > 0)
          {
            oXmlTextWriter.WriteStartElement("CueChunk");
            oXmlTextWriter.WriteAttributeString("size", oCueChunk.dwSize.ToString());
            oXmlTextWriter.WriteAttributeString("numcuepoints", oCueChunk.dwNumPoints.ToString());

            // Write the cue points.
            foreach (WaveStructs.CuePoint oCuePoint in oCueChunk.aCuePoints)
            {
              oXmlTextWriter.WriteStartElement("Point");

              oXmlTextWriter.WriteAttributeString("id", oCuePoint.dwID.ToString());
              oXmlTextWriter.WriteAttributeString("position", oCuePoint.dwPosition.ToString());
              oXmlTextWriter.WriteAttributeString("datachunkid", oCuePoint.sDataChunkID);
              oXmlTextWriter.WriteAttributeString("chunkstart", oCuePoint.dwChunkStart.ToString());
              oXmlTextWriter.WriteAttributeString("blockstart", oCuePoint.dwBlockStart.ToString());
              oXmlTextWriter.WriteAttributeString("sampleoffset", oCuePoint.dwSampleOffset.ToString());

              oXmlTextWriter.WriteEndElement(); // Point
            }

            oXmlTextWriter.WriteEndElement(); // CueChunk
          }

          // LIST chunk info array
          ArrayList aListChunks = oWaveFile.GetListChunkArray();

          foreach (WaveStructs.ListChunk oListChunk in aListChunks)
          {
            oXmlTextWriter.WriteStartElement("ListChunk");
            oXmlTextWriter.WriteAttributeString("type", oListChunk.sTypeID);
            oXmlTextWriter.WriteAttributeString("size", oListChunk.dwSize.ToString());

            switch (oListChunk.sTypeID)
            {
              case "adtl":
                {
                  // Write the labels.
                  foreach (WaveStructs.LablChunk oLablChunk in oListChunk.aLablInfo)
                  {
                    oXmlTextWriter.WriteStartElement("Label");

                    oXmlTextWriter.WriteAttributeString("size", oLablChunk.dwSize.ToString());
                    oXmlTextWriter.WriteAttributeString("cuepointid", oLablChunk.dwCuePointID.ToString());
                    oXmlTextWriter.WriteAttributeString("text", oLablChunk.sText);

                    oXmlTextWriter.WriteEndElement(); // Label
                  }

                  // Write the labeled text.
                  foreach (WaveStructs.LtxtChunk oLtxtChunk in oListChunk.aLtxtInfo)
                  {
                    oXmlTextWriter.WriteStartElement("LabeledText");

                    oXmlTextWriter.WriteAttributeString("size", oLtxtChunk.dwSize.ToString());
                    oXmlTextWriter.WriteAttributeString("cuepointid", oLtxtChunk.dwCuePointID.ToString());
                    oXmlTextWriter.WriteAttributeString("samplelength", oLtxtChunk.dwSampleLength.ToString());
                    oXmlTextWriter.WriteAttributeString("purposeid", oLtxtChunk.dwPurposeID.ToString());
                    oXmlTextWriter.WriteAttributeString("country", oLtxtChunk.wCountry.ToString());
                    oXmlTextWriter.WriteAttributeString("language", oLtxtChunk.wLanguage.ToString());
                    oXmlTextWriter.WriteAttributeString("dialect", oLtxtChunk.wDialect.ToString());
                    oXmlTextWriter.WriteAttributeString("codepage", oLtxtChunk.wCodePage.ToString());
                    oXmlTextWriter.WriteAttributeString("text", oLtxtChunk.sText);

                    oXmlTextWriter.WriteEndElement(); // LabeledText
                  }

                  // Write the "note" chunks.
                  foreach (WaveStructs.NoteChunk oNoteChunk in oListChunk.aNoteInfo)
                  {
                    oXmlTextWriter.WriteStartElement("Note");

                    oXmlTextWriter.WriteAttributeString("size", oNoteChunk.dwSize.ToString());
                    oXmlTextWriter.WriteAttributeString("cuepointid", oNoteChunk.dwCuePointID.ToString());
                    oXmlTextWriter.WriteAttributeString("text", oNoteChunk.sText);

                    oXmlTextWriter.WriteEndElement(); // Note
                  }

                  break;
                }
              case "INFO":
                {
                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sArchive) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Archive");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sArchive);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sArtist) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Artist");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sArtist);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sAlbumArtist) == false)
                  {
                    oXmlTextWriter.WriteStartElement("AlbumArtist");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sAlbumArtist);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sCommissioned) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Commissioned");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sCommissioned);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sComments) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Comments");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sComments);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sCopyright) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Copyright");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sCopyright);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sCreationDate) == false)
                  {
                    oXmlTextWriter.WriteStartElement("CreationDate");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sCreationDate);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sEngineer) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Engineer");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sEngineer);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sGenre) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Genre");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sGenre);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sKeywords) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Keywords");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sKeywords);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sMedium) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Medium");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sMedium);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sTitleName) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Title");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sTitleName);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sProduct) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Product");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sProduct);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sSubject) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Subject");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sSubject);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sSoftware) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Software");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sSoftware);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sSource) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Source");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sSource);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sTechnician) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Technician");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sTechnician);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sITOC) == false)
                  {
                    oXmlTextWriter.WriteStartElement("ITOC");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sITOC);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sTrackNumber) == false)
                  {
                    oXmlTextWriter.WriteStartElement("TrackNumber");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sTrackNumber);
                    oXmlTextWriter.WriteEndElement();
                  }

                  if (String.IsNullOrEmpty(oListChunk.oInfoChunk.sURL) == false)
                  {
                    oXmlTextWriter.WriteStartElement("Website");
                    oXmlTextWriter.WriteAttributeString("value", oListChunk.oInfoChunk.sURL);
                    oXmlTextWriter.WriteEndElement();
                  }

                  break;
                }
            }

            oXmlTextWriter.WriteEndElement(); // ListChunk
          }
        }

        oXmlTextWriter.WriteEndElement(); // WaveFile
      }

      oXmlTextWriter.WriteEndElement(); // Project
      oXmlTextWriter.WriteEndElement(); // Projects
      oXmlTextWriter.Flush();
      oXmlTextWriter.Close();

      // Fill in necessary data
      switch (g_oMergedWaveInfo.wFormatTag)
      {
        case WaveStructs.WAVE_FORMAT_PCM:
          {
            // Uncompressed, canonical WAVE file
            g_oMergedWaveInfo.dwRiffSize = g_oMergedWaveInfo.dwDataSize + 36 + (((g_oMergedWaveInfo.dwDataSize % 2) == 1) ? 1 : Convert.ToUInt32(0)); // Pad a byte in case of odd data size.
            g_oMergedWaveInfo.dwFormatChunkSize = 16;
            g_oMergedWaveInfo.cbSize = 0;
            g_oMergedWaveInfo.wValidBitsPerSample = 0;
            g_oMergedWaveInfo.dwChannelMask = 0;

            break;
          }
        case WaveStructs.WAVE_FORMAT_EXTENSIBLE:
          {
            g_oMergedWaveInfo.dwRiffSize = g_oMergedWaveInfo.dwDataSize + 60 + (((g_oMergedWaveInfo.dwDataSize % 2) == 1) ? 1 : Convert.ToUInt32(0)); // Pad a byte in case of odd data size.
            g_oMergedWaveInfo.dwFormatChunkSize = 40;
            g_oMergedWaveInfo.cbSize = 22;
            g_oMergedWaveInfo.wValidBitsPerSample = g_oMergedWaveInfo.wBitsPerSample;

            switch (g_oMergedWaveInfo.wChannels)
            {
              case 1:
                {
                  // Mono
                  g_oMergedWaveInfo.dwChannelMask = 0x00000000;

                  break;
                }
              case 2:
                {
                  // Stereo
                  g_oMergedWaveInfo.dwChannelMask = WaveStructs.SPEAKER_FRONT_LEFT | WaveStructs.SPEAKER_FRONT_RIGHT;

                  break;
                }
            }

            g_oMergedWaveInfo.SubFormat = WaveStructs.KSDATAFORMAT_SUBTYPE_PCM;

            break;
          }
        case WaveStructs.WAVE_FORMAT_IEEE_FLOAT:
          {
            g_oMergedWaveInfo.dwRiffSize = g_oMergedWaveInfo.dwDataSize + 38 + (((g_oMergedWaveInfo.dwDataSize % 2) == 1) ? 1 : Convert.ToUInt32(0)); // Pad a byte in case of odd data size.
            g_oMergedWaveInfo.dwFormatChunkSize = 18;
            g_oMergedWaveInfo.cbSize = 0;

            break;
          }
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      // Merge the project files but first validate them
      if (ProjectFilesListBox.Items.Count > 0 && ValidateProjectFiles())
      {
        Form2 ProjectDialog = new Form2();
        ProjectDialog.ShowDialog(this);

        if (ProjectDialog.DialogResult == DialogResult.OK)
        {
          // Create the project folder in case it doesn't exist yet
          string sApplicationPath = Path.GetDirectoryName(Application.ExecutablePath);
          string sProjectPath = sApplicationPath + "\\" + ProjectDialog.GetText();
          Directory.CreateDirectory(sProjectPath);

          // Write the files' data to the XML file
          CreateXML(sProjectPath);

          // Create the project
          string sProjectName = Path.GetFileName(sProjectPath);

          if (CreateMergedWaveFile(sProjectPath + "\\" + sProjectName + ".wav"))
          {
            // Show the success dialog.
            ConversionSuccess oSuccessDialog = new ConversionSuccess();
            oSuccessDialog.CenterDialogToParent();
            oSuccessDialog.SetCaption("Merge Successful");
            oSuccessDialog.SetText("The files were successfully merged!");
            oSuccessDialog.SetFolderPath(sProjectPath);
            oSuccessDialog.ShowDialog(this); // Show modal, currently we don't care about the dialog result.
            g_oProgressDialog.Hide();
          }
        }
      }
    }

    private bool CreateMergedWaveFile(string sFilePath)
    {
      // Create the wave file header
      WaveFile oNewWaveFile = CreateNewWaveFileObject();

      if (oNewWaveFile.Initialize(sFilePath, true))
      {
        oNewWaveFile.WriteWaveHeader(g_oMergedWaveInfo);

        // Show a progress bar
        g_oProgressDialog.Text = "Merging Wave Files";
        g_oProgressDialog.CenterDialogToParent();
        g_oProgressDialog.Show(this);
        string sText = String.Empty;
        int nCount = 0;
        uint nDataCount = 0;
        bool bSuccess = false;

        // Now add wave data from all project files to the new one
        foreach (object oItem in ProjectFilesListBox.Items)
        {
          ExtendedListEntry.ExtendedListEntry oListEntryInfo = (ExtendedListEntry.ExtendedListEntry)oItem;
          WaveFile oWaveFile = CreateNewWaveFileObject();

          if (oWaveFile.Initialize(oListEntryInfo.GetSetPath, false))
          {
            byte[] aData = oWaveFile.GetCompleteDataChunkDataArray();

            if (aData != null)
            {
              // Keep track of the added byte count to determine later on whether we need to append a byte or not.
              nDataCount += Convert.ToUInt32(aData.Length);
              bSuccess = oNewWaveFile.AppendWaveData(aData);

              sText = (++nCount).ToString();
              sText += "/";
              sText += ProjectFilesListBox.Items.Count;
              g_oProgressDialog.SetText(sText);
              g_oProgressDialog.SetProgress(100.0 * Convert.ToDouble(nCount) / Convert.ToDouble(ProjectFilesListBox.Items.Count));
              g_oProgressDialog.Update();

              if (bSuccess == false)
              {
                oNewWaveFile.FinalizeFile();
                g_oProgressDialog.Hide();

                return false;
              }
            }
            else
            {
              oNewWaveFile.FinalizeFile();
              g_oProgressDialog.Hide();

              // TODO: Handle a possible error case
              Debug.Assert(false, "Data is null during merge!");

              string sDesc = "A wave file did not return correct data to proceed with the merging procedure. ";
              sDesc += "This could be mainly due to the system being out of memory. If that's the case make sure to delete any partial wave file created by the failed merge procedure. ";
              sDesc += "Additionally provide a sufficient amount of system memory and prevent merging into large files that potentially exceed system limitations. (a 32 bit OS has got a natural boundary of 4 GiB!)";
              ShowMessageBox("Merging Procedure Failed", sDesc);

              return false;
            }
          }
          else
          {
            oNewWaveFile.FinalizeFile();
            g_oProgressDialog.Hide();

            // TODO: Handle a possible error case
            Debug.Assert(false, "Source file did not initialize during merge!");

            return false;
          }
        }

        // Now make sure to append a byte if necessary
        if ((nDataCount % 2) == 1)
        {
          bSuccess = oNewWaveFile.AppendWaveData(new byte[1]);
        }

        oNewWaveFile.FinalizeFile();
        g_oProgressDialog.Hide();

        if (bSuccess == false)
        {
          ShowMessageBox("Writing Data Failed", "Data could not be written to a wave file during merging procedure! Aborting!");
          return false;
        }

        return true;
      }

      // TODO: Handle a possible error case
      oNewWaveFile.FinalizeFile();
      Debug.Assert(false, "Target file did not initialize during merge!");

      return false;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      // Load a project
      openFileDialog1.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
      openFileDialog1.FileName = String.Empty;
      openFileDialog1.Filter = "xml files (*.xml)|*.xml";
      DialogResult result = openFileDialog1.ShowDialog();

      if (result == DialogResult.OK)
      {
        // First clear the content from ProjectFilesListBox
        ProjectFilesListBox.BeginUpdate();
        ProjectFilesListBox.SuspendLayout();
        ArrayList aoListEntryInfos = (ArrayList)ProjectFilesListBox.DataSource;
        aoListEntryInfos.Clear();

        // Now read the project file
        g_sLoadedProjectPath = openFileDialog1.FileName;
        XmlTextReader oReader = new XmlTextReader(g_sLoadedProjectPath);

        g_oProgressDialog.Text = "Loading Project Files";
        g_oProgressDialog.CenterDialogToParent();
        g_oProgressDialog.Show(this);
        string sText = String.Empty;
        int nFilesCount = 0;
        int nCount = 0;

        while (oReader.Read())
        {
          if (oReader.NodeType == XmlNodeType.Element)
          {
            switch (oReader.Name)
            {
              case "Project":
                {
                  g_sLoadedProjectName = oReader.GetAttribute("name");
                  g_oMergedWaveInfo.wFormatTag = Convert.ToUInt16(oReader.GetAttribute("format"));
                  g_oMergedWaveInfo.wChannels = Convert.ToUInt16(oReader.GetAttribute("channels"));
                  g_oMergedWaveInfo.dwSamplesPerSec = Convert.ToUInt32(oReader.GetAttribute("samplespersec"));
                  g_oMergedWaveInfo.dwAvgBytesPerSec = Convert.ToUInt32(oReader.GetAttribute("avgbytespersec"));
                  g_oMergedWaveInfo.wBlockAlign = Convert.ToUInt16(oReader.GetAttribute("blockalign"));
                  g_oMergedWaveInfo.wBitsPerSample = Convert.ToUInt16(oReader.GetAttribute("bitspersample"));
                  nFilesCount = Convert.ToInt32(oReader.GetAttribute("filescount"));

                  // Fill in necessary data.
                  switch (g_oMergedWaveInfo.wFormatTag)
                  {
                    case WaveStructs.WAVE_FORMAT_PCM:
                      {
                        // Uncompressed, canonical WAVE file.
                        g_oMergedWaveInfo.dwFormatChunkSize = 16;
                        g_oMergedWaveInfo.cbSize = 0;
                        g_oMergedWaveInfo.wValidBitsPerSample = 0;
                        g_oMergedWaveInfo.dwChannelMask = 0;

                        break;
                      }
                    case WaveStructs.WAVE_FORMAT_EXTENSIBLE:
                      {
                        g_oMergedWaveInfo.dwFormatChunkSize = 40;
                        g_oMergedWaveInfo.cbSize = 22;
                        g_oMergedWaveInfo.wValidBitsPerSample = 24;

                        switch (g_oMergedWaveInfo.wChannels)
                        {
                          case 1:
                            {
                              // Mono
                              g_oMergedWaveInfo.dwChannelMask = 0x00000000;

                              break;
                            }
                          case 2:
                            {
                              // Stereo
                              g_oMergedWaveInfo.dwChannelMask = WaveStructs.SPEAKER_FRONT_LEFT | WaveStructs.SPEAKER_FRONT_RIGHT;

                              break;
                            }
                        }

                        g_oMergedWaveInfo.SubFormat = WaveStructs.KSDATAFORMAT_SUBTYPE_PCM;

                        break;
                      }
                    case WaveStructs.WAVE_FORMAT_IEEE_FLOAT:
                      {
                        g_oMergedWaveInfo.dwFormatChunkSize = 18;
                        g_oMergedWaveInfo.cbSize = 0;

                        break;
                      }
                  }

                  break;
                }
              case "WaveFile":
                {
                  string sFileName = oReader.GetAttribute("name");
                  string sFilePath = oReader.GetAttribute("path");
                  ExtendedListEntry.ExtendedListEntry oListEntryInfo = new ExtendedListEntry.ExtendedListEntry(sFileName, sFilePath);
                  aoListEntryInfos.Add(oListEntryInfo);

                  sText = (++nCount).ToString();
                  sText += "/";
                  sText += nFilesCount;
                  g_oProgressDialog.SetText(sText);
                  g_oProgressDialog.SetProgress(100.0 * Convert.ToDouble(nCount) / Convert.ToDouble(nFilesCount));
                  g_oProgressDialog.Update();

                  break;
                }
            }
          }
        }

        // Release the xml file again
        oReader.Close();

        // Clear the project path string again in case this is not a valid project file
        if (String.IsNullOrEmpty(g_sLoadedProjectName) == true)
        {
          g_sLoadedProjectPath = String.Empty;
        }

        LoadedProjectLabel.ForeColor = Color.Goldenrod;
        LoadedProjectLabel.Text = g_sLoadedProjectName;
        ProjectFilesListBox.DataSource = null;
        ProjectFilesListBox.DisplayMember = "GetSetName";
        ProjectFilesListBox.ValueMember = "GetSetPath";
        ProjectFilesListBox.DataSource = aoListEntryInfos;
        ProjectFilesListBox.ResumeLayout();
        ProjectFilesListBox.EndUpdate();
        g_oProgressDialog.Hide();
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      // Split the project into its single files again.
      if (String.IsNullOrEmpty(g_sLoadedProjectPath) == false)
      {
        uint nOffset = 0;
        string sProjectWaveFilePath = Path.ChangeExtension(g_sLoadedProjectPath, ".wav");
        WaveFile oProjectWaveFile = CreateNewWaveFileObject();

        if (oProjectWaveFile.Initialize(sProjectWaveFilePath, false))
        {
          XmlTextReader oCheckDataSizeReader = new XmlTextReader(g_sLoadedProjectPath);

          // Make sure the project file's data layout hasn't been changed before continuing.U
          uint nReportedDataSize = 0;

          while (oCheckDataSizeReader.Read())
          {
            if (oCheckDataSizeReader.NodeType == XmlNodeType.Element)
            {
              switch (oCheckDataSizeReader.Name)
              {
                case "WaveFile":
                  {
                    uint nDataSize = Convert.ToUInt32(oCheckDataSizeReader.GetAttribute("datasize"));

                    if (nDataSize > 0)
                    {
                      nReportedDataSize += nDataSize;
                    }
                    else
                    {
                      ShowMessageBox("Wave File Entry Invalid", "Looks like one of the wave file entries in the project XML has got an invalid \"datasize\" attribute!");
                      return;
                    }

                    break;
                  }
              }
            }
          }

          if (nReportedDataSize > 0 && oProjectWaveFile.GetDataChunk().dwSize != nReportedDataSize)
          {
            string sErrorText = "Looks like the the project wave file's data layout has been changed.\n";
            sErrorText += string.Format("\nReported Size: {0} byte\nActual Size: {1} byte\n", nReportedDataSize, oProjectWaveFile.GetDataChunk().dwSize);
            sErrorText += "\nAborting splitting procedure!";
            ShowMessageBox("Project Wave File Invalid", sErrorText);
            return;
          }

          // Show a progress bar.
          g_oProgressDialog.Text = "Splitting Project Wave File";
          g_oProgressDialog.CenterDialogToParent();
          g_oProgressDialog.Show(this);
          string sText = String.Empty;
          int nCount = 0;
          int nFilesCount = 0;

          XmlTextReader oReader = new XmlTextReader(g_sLoadedProjectPath);

          while (oReader.Read())
          {
            if (oReader.NodeType == XmlNodeType.Element)
            {
              switch (oReader.Name)
              {
                case "Project":
                  {
                    nFilesCount = Convert.ToInt32(oReader.GetAttribute("filescount"));

                    break;
                  }
                case "WaveFile":
                  {
                    string sFileName = oReader.GetAttribute("name");
                    string sFilePath = oReader.GetAttribute("path");

                    if (oReader.MoveToAttribute("datasize"))
                    {
                      string sProjectPathWithoutFile = Path.GetDirectoryName(g_sLoadedProjectPath);
                      string sNewFilePath = sProjectPathWithoutFile + "\\" + sFileName;

                      // Check whether the file already exists or not and rename it if so.
                      if (File.Exists(sNewFilePath) == true)
                      {
                        string sPath = Path.GetDirectoryName(sFilePath);
                        sPath = sPath.Substring(sPath.LastIndexOf("\\") + 1);
                        sFileName = sFileName.Substring(0, sFileName.LastIndexOf("."));
                        sNewFilePath = sProjectPathWithoutFile + "\\" + sFileName + "(duplicate! check XML file at line " + oReader.LineNumber.ToString() + " for more information!)" + ".wav";
                      }

                      WaveFile oNewWaveFile = CreateNewWaveFileObject();

                      if (oNewWaveFile.Initialize(sNewFilePath, true))
                      {
                        // Fill in necessary data.
                        uint nDataSize = Convert.ToUInt32(oReader.Value);
                        WaveStructs.NewWaveInfo oNewWaveInfo = new WaveStructs.NewWaveInfo(g_oMergedWaveInfo);

                        oNewWaveInfo.dwDataSize = nDataSize;

                        switch (g_oMergedWaveInfo.wFormatTag)
                        {
                          case WaveStructs.WAVE_FORMAT_PCM:
                            {
                              // Uncompressed, canonical WAVE file.
                              oNewWaveInfo.dwRiffSize = oNewWaveInfo.dwDataSize + 36 + (((oNewWaveInfo.dwDataSize % 2) == 1) ? 1 : Convert.ToUInt32(0)); // Pad a byte in case of odd data size.

                              break;
                            }
                          case WaveStructs.WAVE_FORMAT_EXTENSIBLE:
                            {
                              oNewWaveInfo.dwRiffSize = oNewWaveInfo.dwDataSize + 60 + (((oNewWaveInfo.dwDataSize % 2) == 1) ? 1 : Convert.ToUInt32(0)); // Pad a byte in case of odd data size.
                              oNewWaveInfo.wValidBitsPerSample = oNewWaveInfo.wBitsPerSample; // Only WAVE_FORMAT_EXTENSIBLE needs this.

                              break;
                            }
                          case WaveStructs.WAVE_FORMAT_IEEE_FLOAT:
                            {
                              oNewWaveInfo.dwRiffSize = oNewWaveInfo.dwDataSize + 38 + (((oNewWaveInfo.dwDataSize % 2) == 1) ? 1 : Convert.ToUInt32(0)); // Pad a byte in case of odd data size.

                              break;
                            }
                        }

                        // In case there are sub-chunks of interest make sure to also update the RIFF size.
                        oReader.MoveToElement();
                        XmlReader oSubChunkReader = oReader.ReadSubtree();

                        while (oSubChunkReader.Read())
                        {
                          if (oSubChunkReader.NodeType == XmlNodeType.Element)
                          {
                            switch (oSubChunkReader.Name)
                            {
                              case "FactChunk":
                                {
                                  uint nSize                           = Convert.ToUInt32(oSubChunkReader.GetAttribute("size"));
                                  oNewWaveInfo.dwRiffSize              += nSize + 8;
                                  oNewWaveInfo.oFactChunk              = new WaveStructs.FactChunk();
                                  oNewWaveInfo.oFactChunk.dwSize       = nSize;
                                  oNewWaveInfo.oFactChunk.dwNumSamples = Convert.ToUInt32(oSubChunkReader.GetAttribute("numsamples"));
                                  oNewWaveInfo.oFactChunk.sID          = "fact";

                                  break;
                                }
                              case "CueChunk":
                                {
                                  uint nNumPoints = Convert.ToUInt32(oSubChunkReader.GetAttribute("numcuepoints"));

                                  if (nNumPoints > 0)
                                  {
                                    uint nSize                         = Convert.ToUInt32(oSubChunkReader.GetAttribute("size"));
                                    oNewWaveInfo.dwRiffSize            += nSize + 8;
                                    oNewWaveInfo.oCueChunk             = new WaveStructs.CueChunk();
                                    oNewWaveInfo.oCueChunk.dwSize      = nSize;
                                    oNewWaveInfo.oCueChunk.sID         = "cue ";
                                    oNewWaveInfo.oCueChunk.dwNumPoints = nNumPoints;

                                    oSubChunkReader.MoveToElement();
                                    XmlReader oCuePointReader = oSubChunkReader.ReadSubtree();


                                    while (oCuePointReader.Read())
                                    {
                                      if (oSubChunkReader.NodeType == XmlNodeType.Element && oCuePointReader.Name == "Point")
                                      {
                                        WaveStructs.CuePoint oCuePoint = new WaveStructs.CuePoint();
                                        oCuePoint.dwID                 = Convert.ToUInt32(oCuePointReader.GetAttribute("id"));
                                        oCuePoint.dwPosition           = Convert.ToUInt32(oCuePointReader.GetAttribute("position"));
                                        oCuePoint.sDataChunkID         = oCuePointReader.GetAttribute("datachunkid");
                                        oCuePoint.dwChunkStart         = Convert.ToUInt32(oCuePointReader.GetAttribute("chunkstart"));
                                        oCuePoint.dwBlockStart         = Convert.ToUInt32(oCuePointReader.GetAttribute("blockstart"));
                                        oCuePoint.dwSampleOffset       = Convert.ToUInt32(oCuePointReader.GetAttribute("sampleoffset"));

                                        oNewWaveInfo.oCueChunk.aCuePoints.Add(oCuePoint);
                                      }
                                    }
                                  }

                                  break;
                                }
                              case "ListChunk":
                                {
                                  // For now we carry only cue points along therefore type "adtl" is of interest only.
                                  string sListType = oReader.GetAttribute("type");

                                  if (sListType == "adtl")
                                  {
                                    uint nSize                      = Convert.ToUInt32(oSubChunkReader.GetAttribute("size"));
                                    oNewWaveInfo.dwRiffSize         += nSize + 8;
                                    oNewWaveInfo.oListChunk         = new WaveStructs.ListChunk();
                                    oNewWaveInfo.oListChunk.dwSize  = nSize;
                                    oNewWaveInfo.oListChunk.sID     = "LIST";
                                    oNewWaveInfo.oListChunk.sTypeID = "adtl";

                                    oSubChunkReader.MoveToElement();
                                    XmlReader oLabelsReader = oSubChunkReader.ReadSubtree();

                                    while (oLabelsReader.Read())
                                    {
                                      if (oLabelsReader.NodeType == XmlNodeType.Element)
                                      {
                                        switch (oLabelsReader.Name)
                                        {
                                          case "Label":
                                            {
                                              WaveStructs.LablChunk oLablChunk = new WaveStructs.LablChunk();
                                              oLablChunk.sID                   = "labl";
                                              oLablChunk.dwSize                = Convert.ToUInt32(oLabelsReader.GetAttribute("size"));
                                              oLablChunk.dwCuePointID          = Convert.ToUInt32(oLabelsReader.GetAttribute("cuepointid"));
                                              oLablChunk.sText                 = oLabelsReader.GetAttribute("text");

                                              oNewWaveInfo.oListChunk.aLablInfo.Add(oLablChunk);

                                              break;
                                            }
                                          case "LabeledText":
                                            {
                                              WaveStructs.LtxtChunk oLtxtChunk = new WaveStructs.LtxtChunk();
                                              oLtxtChunk.sID                   = "ltxt";
                                              oLtxtChunk.dwSize                = Convert.ToUInt32(oLabelsReader.GetAttribute("size"));
                                              oLtxtChunk.dwCuePointID          = Convert.ToUInt32(oLabelsReader.GetAttribute("cuepointid"));
                                              oLtxtChunk.dwSampleLength        = Convert.ToUInt32(oLabelsReader.GetAttribute("samplelength"));
                                              oLtxtChunk.dwPurposeID           = Convert.ToUInt32(oLabelsReader.GetAttribute("purposeid"));
                                              oLtxtChunk.wCountry              = Convert.ToUInt16(oLabelsReader.GetAttribute("country"));
                                              oLtxtChunk.wLanguage             = Convert.ToUInt16(oLabelsReader.GetAttribute("language"));
                                              oLtxtChunk.wDialect              = Convert.ToUInt16(oLabelsReader.GetAttribute("dialect"));
                                              oLtxtChunk.wCodePage             = Convert.ToUInt16(oLabelsReader.GetAttribute("codepage"));
                                              oLtxtChunk.sText                 = oLabelsReader.GetAttribute("text");

                                              oNewWaveInfo.oListChunk.aLtxtInfo.Add(oLtxtChunk);

                                              break;
                                            }
                                          case "Note":
                                            {
                                              WaveStructs.NoteChunk oNoteChunk = new WaveStructs.NoteChunk();
                                              oNoteChunk.sID                   = "note";
                                              oNoteChunk.dwSize                = Convert.ToUInt32(oLabelsReader.GetAttribute("size"));
                                              oNoteChunk.dwCuePointID          = Convert.ToUInt32(oLabelsReader.GetAttribute("cuepointid"));
                                              oNoteChunk.sText                 = oLabelsReader.GetAttribute("text");

                                              oNewWaveInfo.oListChunk.aNoteInfo.Add(oNoteChunk);

                                              break;
                                            }
                                        }
                                      }
                                    }
                                  }

                                  break;
                                }
                            }
                          }
                        }

                        // Create the new wave file header.
                        oNewWaveFile.WriteWaveHeader(oNewWaveInfo);
                        byte[] aData  = oProjectWaveFile.GetPartDataChunkDataArray(nOffset, nDataSize);
                        bool bSuccess = oNewWaveFile.AppendWaveData(aData);

                        if (bSuccess)
                        {
                          // Now make sure to append a byte if necessary.
                          if ((nDataSize % 2) == 1)
                          {
                            bSuccess = oNewWaveFile.AppendWaveData(new byte[1]);
                          }

                          if (bSuccess)
                          {
                            oNewWaveFile.WriteAfterWaveData(oNewWaveInfo);
                            oNewWaveFile.FinalizeFile();

                            // Traverse through the project wave file.
                            nOffset += nDataSize;

                            // Update the progress bar.
                            sText = (++nCount).ToString();
                            sText += "/";
                            sText += nFilesCount;
                            g_oProgressDialog.SetText(sText);
                            g_oProgressDialog.SetProgress(100.0 * Convert.ToDouble(nCount) / Convert.ToDouble(nFilesCount));
                            g_oProgressDialog.Update();
                          }
                        }

                        if (bSuccess == false)
                        {
                          // Release the project wave file again.
                          oProjectWaveFile.FinalizeFile();

                          // Release the xml file again.
                          oReader.Close();

                          // Hide the progress bar dialog again.
                          g_oProgressDialog.BringToFront();
                          g_oProgressDialog.Focus();
                          g_oProgressDialog.Hide();

                          ShowMessageBox("Writing Data Failed", "Data could not be written to a wave file during splitting procedure! Aborting!");

                          return;
                        }
                      }
                    }

                    break;
                  }
              }
            }
          }

          // Release the project wave file again.
          oProjectWaveFile.FinalizeFile();

          // Release the xml file again.
          oReader.Close();

          // Hide the progress bar dialog again.
          g_oProgressDialog.BringToFront();
          g_oProgressDialog.Focus();
          g_oProgressDialog.Hide();

          // Show the success dialog.
          ConversionSuccess oSuccessDialog = new ConversionSuccess();
          oSuccessDialog.CenterDialogToParent();
          oSuccessDialog.SetCaption("Split Successful");
          oSuccessDialog.SetText("The files were successfully split!");
          string sFolderPath = Path.GetDirectoryName(g_sLoadedProjectPath);
          oSuccessDialog.SetFolderPath(sFolderPath);
          oSuccessDialog.ShowDialog(this); // Show modal, currently we don't care about the dialog result.
        }
      }
      else
      {
        ShowMessageBox("No Project Loaded", "Before splitting a project into its contents make sure to load it!");
      }
    }

    private void button7_Click(object sender, EventArgs e)
    {
      Form3 SettingsDialog = new Form3();

      // Set current values
      SettingsDialog.SetValues(g_oMergedWaveInfo);
      SettingsDialog.ShowDialog(this);

      if (SettingsDialog.DialogResult == DialogResult.OK)
      {
        g_oMergedWaveInfo = SettingsDialog.GetSettings();
      }
    }

    private bool ValidateProjectFiles()
    {
      g_oProgressDialog.Text = "Validating Wave Files";
      g_oProgressDialog.CenterDialogToParent();
      g_oProgressDialog.Show(this);
      string sText        = String.Empty;
      int nCount          = 0;
      uint nBadFilesCount = 0;
      bool bResult        = true;

      ProjectFilesListBox.BeginUpdate();
      ProjectFilesListBox.SuspendLayout();

      foreach (object oItem in ProjectFilesListBox.Items)
      {
        ExtendedListEntry.ExtendedListEntry oListEntryInfo = (ExtendedListEntry.ExtendedListEntry)oItem;

        // First assume this entry is valid, let the code below prove otherwise
        oListEntryInfo.GetSetValid = true;

        WaveFile oWaveFile = CreateNewWaveFileObject();

        if (oWaveFile.Initialize(oListEntryInfo.GetSetPath, false))
        {
          // Format chunk info
          WaveStructs.FormatChunk oFormatChunk = oWaveFile.GetFormatChunk();
          ushort wFormatTag                    = oFormatChunk.wFormatTag;
          ushort wChannels                     = oFormatChunk.wChannels;
          uint dwSamplesPerSec                 = oFormatChunk.dwSamplesPerSec;
          ushort wBitsPerSample                = oFormatChunk.wBitsPerSample;

          if (wFormatTag != g_oMergedWaveInfo.wFormatTag ||
                wChannels != g_oMergedWaveInfo.wChannels ||
                dwSamplesPerSec != g_oMergedWaveInfo.dwSamplesPerSec ||
                wBitsPerSample != g_oMergedWaveInfo.wBitsPerSample)
          {
            oListEntryInfo.GetSetValid = false;
            ++nBadFilesCount;
          }
        }
        else
        {
          oListEntryInfo.GetSetValid = false;
          ++nBadFilesCount;
        }

        oWaveFile.FinalizeFile();

        sText = (++nCount).ToString();
        sText += "/";
        sText += ProjectFilesListBox.Items.Count;
        g_oProgressDialog.SetText(sText);
        g_oProgressDialog.SetProgress(100.0 * Convert.ToDouble(nCount) / Convert.ToDouble(ProjectFilesListBox.Items.Count));
        g_oProgressDialog.Update();
      }

      g_oProgressDialog.Hide();

      if (nBadFilesCount > 0)
      {
        string sTemp = String.Format("There are {0:d} project files that do not match the project's settings!\nMismatching files will be displayed in red for your convenience.", nBadFilesCount);
        ShowMessageBox("Mismatching Files", sTemp);

        bResult = false;
      }

      ProjectFilesListBox.ResumeLayout();
      ProjectFilesListBox.EndUpdate();

      return bResult;
    }

    private void InvalidateProject()
    {
      g_sLoadedProjectName         = String.Empty;
      g_sLoadedProjectPath         = String.Empty;
      LoadedProjectLabel.ForeColor = Color.DimGray;
      LoadedProjectLabel.Text      = "NoProjectLoaded";
    }

    private void button8_Click(object sender, EventArgs e)
    {
      int nTopIndex = ProjectFilesListBox.TopIndex;
      ArrayList aoListEntryInfos = (ArrayList)ProjectFilesListBox.DataSource;

      foreach (object oItem in ProjectFilesListBox.Items)
      {
        ExtendedListEntry.ExtendedListEntry oListEntryInfo = (ExtendedListEntry.ExtendedListEntry)oItem;

        if (oListEntryInfo.GetSetValid == false)
        {
          aoListEntryInfos.Remove(oItem);
        }
      }

      aoListEntryInfos.Sort();
      ProjectFilesListBox.BeginUpdate();
      ProjectFilesListBox.SuspendLayout();
      ProjectFilesListBox.DataSource = null;
      ProjectFilesListBox.DisplayMember = "GetSetName";
      ProjectFilesListBox.ValueMember = "GetSetPath";
      ProjectFilesListBox.DataSource = aoListEntryInfos;
      ProjectFilesListBox.TopIndex = (ProjectFilesListBox.Items.Count > nTopIndex) ? nTopIndex : ProjectFilesListBox.Items.Count;
      ProjectFilesListBox.ResumeLayout();
      ProjectFilesListBox.EndUpdate();
    }

    private void ShowToolTip(ExtendedListEntry.ExtendedListEntry oListEntryInfo, ExtendedListBox.ExtendedListBox oWaveHandlerListBox)
    {
      WaveFile oInfoWaveFile = CreateNewWaveFileObject();

      if (oInfoWaveFile.Initialize(oListEntryInfo.GetSetPath, false))
      {
        WaveStructs.FormatChunk oFormatChunk = oInfoWaveFile.GetFormatChunk();
        string sSignedOrFloat = "unknown size";

        switch (oFormatChunk.wBitsPerSample)
        {
          case 8:
            {
              sSignedOrFloat = "PCM unsigned 8 bit";

              break;
            }
          case 16:
            {
              sSignedOrFloat = "PCM signed 16 bit";

              break;
            }
          case 24:
            {
              sSignedOrFloat = "PCM signed 24 bit";

              break;
            }
          case 32:
            {
              switch (oFormatChunk.wFormatTag)
              {
                case WaveStructs.WAVE_FORMAT_EXTENSIBLE:
                  {
                    sSignedOrFloat = "PCM signed 32 bit";

                    break;
                  }
                case WaveStructs.WAVE_FORMAT_IEEE_FLOAT:
                  {
                    sSignedOrFloat = "IEEE float signed 32 bit";

                    break;
                  }
              }

              break;
            }
        }

        string sText = string.Format("{0}\n\nSample Rate:\t{1} Hz\nSample Size:\t{2}\nChannel Count:\t{3}\nData Format:\t{4}\n", oListEntryInfo.GetSetPath, oFormatChunk.dwSamplesPerSec, sSignedOrFloat, oFormatChunk.wChannels, oFormatChunk.wFormatTag);
        toolTip1.SetToolTip(oWaveHandlerListBox, sText);
      }
      else
      {
        toolTip1.SetToolTip(oWaveHandlerListBox, oListEntryInfo.GetSetPath);
      }
    }
  }

  // This event stuff must be put after the main Form1 class otherwise the Designer breaks.
  public delegate void GeneralMessageEventHandler(object sender, WaveHandlerEventArgs e);
  public delegate DialogResult ExtendedMessageEventHandler(object sender, WaveHandlerEventArgs e);

  public class WaveHandlerEventArgs : EventArgs
  {
    public WaveHandlerEventArgs(string sCaption, string sText, string sOkButtonText = "Ok", string sCancelButtonText = "Cancel")
    {
      sDialogCaption          = sCaption;
      sEventText              = sText;
      sDialogOkButtonText     = sOkButtonText;
      sDialogCancelButtonText = sCancelButtonText;
    }

    public string sDialogCaption;
    public string sEventText;
    public string sDialogOkButtonText;
    public string sDialogCancelButtonText;
  }
}
