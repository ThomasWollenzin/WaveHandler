using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WaveHandler
{
	class WaveFile : IDisposable
	{
		public event GeneralMessageEventHandler GeneralMessageEvent;
		public event ExtendedMessageEventHandler ExtendedMessageEvent;

		BinaryReader oBinaryReader;
		BinaryWriter oBinaryWriter;

		WaveStructs.RiffChunk oRiff     = new WaveStructs.RiffChunk();
		WaveStructs.FormatChunk oFormat = new WaveStructs.FormatChunk();
		WaveStructs.FactChunk oFact     = new WaveStructs.FactChunk();
		WaveStructs.DataChunk oData     = new WaveStructs.DataChunk();
		WaveStructs.CueChunk oCue       = new WaveStructs.CueChunk();

		ArrayList aLists = new ArrayList();
		ArrayList aoUnknownChunkInfos = new ArrayList();

		public WaveFile()
		{
		}

		public void Dispose()
		{
			FinalizeFile();
		}

		protected virtual void ShowMessageBox(string sCaption, string sText)
		{
			if (GeneralMessageEvent != null)
			{
				GeneralMessageEvent(this, new WaveHandlerEventArgs(sCaption, sText));
			}
		}

		protected virtual DialogResult ShowMessageBoxExt(string sCaption, string sText, string sOkButtonText = "Ok", string sCancelButtonText = "Cancel")
		{
			DialogResult oResult = DialogResult.None;

			if (ExtendedMessageEvent != null)
			{
				oResult = ExtendedMessageEvent(this, new WaveHandlerEventArgs(sCaption, sText, sOkButtonText, sCancelButtonText));
			}

			return oResult;
		}

		public bool Initialize(string sFileName, bool bNew)
		{
			// TODO: Check whether it's necessary to manually close the file streams.
			if (bNew)
			{
				try
				{
					oBinaryWriter = new BinaryWriter(new FileStream(sFileName, FileMode.Create, FileAccess.Write), Encoding.Default);
				}
				catch (System.Exception oException)
				{
					string sErrorMessage = oException.GetType().Name + " while trying to write file: \n";
					sErrorMessage += sFileName;

					// Will be modal!
					ShowMessageBox(oException.GetType().Name + " Occurred!", sErrorMessage);

					return false;
				}
			}
			else
			{
				try
				{
					oBinaryReader = new BinaryReader(new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.Default);
				}
				catch (System.Exception oException)
				{
					string sErrorMessage = oException.GetType().Name + " while trying to read file: \n";
					sErrorMessage += sFileName;

					// Will be modal!
					ShowMessageBox(oException.GetType().Name + " Occurred!", sErrorMessage);

					return false;
				}

				long nDiscrepancy = 0;

				// First add the RIFF header as it's always the first one. (describes the file)
				oRiff.sID = Encoding.Default.GetString(oBinaryReader.ReadBytes(4));
				oRiff.dwSize = oBinaryReader.ReadUInt32() + 8; // 8 for adding RIFF ID and RIFF size fields.

				// Make sure the reported size actually matches the file's size.
				if (oBinaryReader.BaseStream.Length < Convert.ToInt64(oRiff.dwSize))
				{
					nDiscrepancy = Convert.ToInt64(oRiff.dwSize) - oBinaryReader.BaseStream.Length;

					string sTemp = "Following file is badly formatted!\n";
					sTemp += sFileName;
					sTemp += "\n";
					sTemp += "Reported Size: ";
					sTemp += oRiff.dwSize.ToString();
					sTemp += " Actual Size: ";
					sTemp += oBinaryReader.BaseStream.Length.ToString();
					sTemp += "\nIn the case of proceeding be aware that ";
					sTemp += nDiscrepancy;
					sTemp += " byte will be cut off the end of the data chunk!";
					sTemp += " Do you want to skip it or try to process it anyway?";

					DialogResult oResult = ShowMessageBoxExt("Bad File Size", sTemp, "Process", "Skip");

					switch (oResult)
					{
						case DialogResult.OK:
							{
								oRiff.dwSize = Convert.ToUInt32(oBinaryReader.BaseStream.Length);

								break;
							}
						case DialogResult.Cancel:
							{
								return false;
							}
					}
				}

				oRiff.sFormat = Encoding.Default.GetString(oBinaryReader.ReadBytes(4));

				while ((uint)GetPosition() < oRiff.dwSize)
				{
					string sChunkID = Encoding.Default.GetString(oBinaryReader.ReadBytes(4));

					// Make sure to not read again in case the last read operation reached the end of the stream.
					if (GetPosition() < Convert.ToInt64(oRiff.dwSize))
					{
						switch (sChunkID)
						{
							case "fmt ":
								{
									oFormat.sID              = sChunkID;
									oFormat.dwSize           = oBinaryReader.ReadUInt32();
									oFormat.nPosition        = GetPosition();
									oFormat.wFormatTag       = oBinaryReader.ReadUInt16();
									oFormat.wChannels        = oBinaryReader.ReadUInt16();
									oFormat.dwSamplesPerSec  = oBinaryReader.ReadUInt32();
									oFormat.dwAvgBytesPerSec = oBinaryReader.ReadUInt32();
									oFormat.wBlockAlign      = oBinaryReader.ReadUInt16();
									oFormat.wBitsPerSample   = oBinaryReader.ReadUInt16();

									switch (oFormat.wFormatTag)
									{
										case WaveStructs.WAVE_FORMAT_EXTENSIBLE:
											{
												oFormat.cbSize              = oBinaryReader.ReadUInt16();
												oFormat.wValidBitsPerSample = oBinaryReader.ReadUInt16();
												oFormat.dwChannelMask       = oBinaryReader.ReadUInt32();
												oFormat.SubFormat           = new Guid(oBinaryReader.ReadBytes(16));

												break;
											}
										case WaveStructs.WAVE_FORMAT_IEEE_FLOAT:
											{
												// Here the chunk size should be 18 but can be 16 byte. Let us know in case it's bigger than 18 byte. This could be an improperly formated wave file.
												Debug.Assert(oFormat.dwSize < 19, "The format chunk's size is bigger than 18 byte!");

												// Only read the cbSize field if the size indicates it's actually there.
												if (oFormat.dwSize > 16)
												{
													oFormat.cbSize = oBinaryReader.ReadUInt16();
												}

												break;
											}
										default:
											{
												if (oFormat.dwSize == 18)
												{
													// TODO: Double check what this is about! Uncompressed PCM with fmt chunk size of 18 instead of 16??
													// Badly formatted maybe?
													oFormat.cbSize = oBinaryReader.ReadUInt16();
												}

												// This wave file has a format tag of 1 set even though it carries 24bit encoded data.
												// That's not really bad thing just not complying to the standard. We convert the file back to the standard.
												if (oFormat.wBitsPerSample == 24)
												{
													oFormat.wFormatTag = WaveStructs.WAVE_FORMAT_EXTENSIBLE;
												}

												break;
											}
									}

									// Assert on a reader's bad position so we know whether this is a badly formatted file or unsupported format.
									Debug.Assert(oBinaryReader.BaseStream.Position == (oFormat.nPosition + Convert.ToInt64(oFormat.dwSize)), "This is a badly formatted file or unsupported format!");

									// To prevent crashes make sure to set the reader's position to the beginning of the next chunk.
									oBinaryReader.BaseStream.Seek(oFormat.nPosition + Convert.ToInt64(oFormat.dwSize), SeekOrigin.Begin);

									break;
								}
							case "fact":
								{
									oFact.sID          = sChunkID;
									oFact.dwSize       = oBinaryReader.ReadUInt32();
									oFact.nPosition    = GetPosition();
									oFact.dwNumSamples = oBinaryReader.ReadUInt32();

									oBinaryReader.BaseStream.Seek(oFact.nPosition + Convert.ToInt64(oFact.dwSize) + Convert.ToInt64(((oFact.dwSize % 2) == 1) ? 1 : 0), SeekOrigin.Begin);

									break;
								}
							case "data":
								{
									oData.sID       = sChunkID;
									oData.dwSize    = Convert.ToUInt32(Convert.ToInt64(oBinaryReader.ReadUInt32()) - nDiscrepancy);
									oData.nPosition = GetPosition();

									// Skip directly forward to the next chunk or the end of the file, make sure to take a padded byte into account.
									oBinaryReader.BaseStream.Seek(oData.nPosition + Convert.ToInt64(oData.dwSize) + Convert.ToInt64(((oData.dwSize % 2) == 1) ? 1 : 0), SeekOrigin.Begin);

									break;
								}
							case "GOBL":
								{
									break;
								}
							case "CDif":
								{
									// No clue what this chunk is about. Sound Forge seems to be creating it for storing metadata but looks like the data is 0.
									uint dwChunkSize    = oBinaryReader.ReadUInt32();
									long nChunkPosition = GetPosition();

									// Seek forward to the next chunk location.
									oBinaryReader.BaseStream.Seek(nChunkPosition + Convert.ToInt64(dwChunkSize), SeekOrigin.Begin);

									break;
								}
							case "LIST":
								{
									uint nActualSize    = 0;
									uint nLastReadBytes = 0;
									WaveStructs.ListChunk oList = new WaveStructs.ListChunk();

									try
									{
										// Support LIST types "adtl" and "INFO" only for now.
										oList.sID       = sChunkID;
										oList.dwSize    = oBinaryReader.ReadUInt32();
										uint nListChunkSizeForIteration = oList.dwSize;
										oList.nPosition = GetPosition();
										oList.sTypeID   = Encoding.Default.GetString(oBinaryReader.ReadBytes(4));
										nLastReadBytes  = 4;
										nActualSize     += nLastReadBytes;

										switch (oList.sTypeID)
										{
											case "adtl":
												{
													while (oBinaryReader.BaseStream.Position < oList.nPosition + Convert.ToInt64(nListChunkSizeForIteration + (((nListChunkSizeForIteration % 2) == 0) ? 0 : 1)))
													{
														sChunkID    = Encoding.Default.GetString(oBinaryReader.ReadBytes(4));
														nActualSize += 4;

														switch (sChunkID)
														{
															case "labl":
																{
																	WaveStructs.LablChunk oLablChunk = new WaveStructs.LablChunk();
																	oLablChunk.sID                   = sChunkID;
																	oLablChunk.dwSize                = oBinaryReader.ReadUInt32();
																	nLastReadBytes                   = 4;
																	nActualSize                      += nLastReadBytes;
																	oLablChunk.dwCuePointID          = oBinaryReader.ReadUInt32();
																	nLastReadBytes                   = 4;
																	nActualSize                      += nLastReadBytes;

																	// This means there is text.
																	if (oLablChunk.dwSize > 4)
																	{
																		// First get the text.
																		string sText = Encoding.Default.GetString(oBinaryReader.ReadBytes(Convert.ToInt32(oLablChunk.dwSize) - 4));

																		// Then read a possible padding byte.
																		if ((oLablChunk.dwSize % 2) != 0)
																		{
																			nLastReadBytes = 1;
																			nActualSize    += nLastReadBytes;
																			oBinaryReader.ReadByte();
																		}

																		uint nAdditionalByteCount = 0;

																		// The null termination's index.
																		int nIndex = sText.IndexOf('\0', 0);

																		if (nIndex > 0)
																		{
																			nLastReadBytes       = Convert.ToUInt32(nIndex + 1);
																			nActualSize          += nLastReadBytes;
																			oLablChunk.sText     = sText.Substring(0, nIndex);
																			nAdditionalByteCount = 1;
																		}
																		else
																		{
																			// This is not a bad thing! I would just like to get informed.
																			Debug.Assert(false, "LablChunk without actual text!");
																		}

																		// Here we make sure to adjust the labl chunk's size according to what we trimmed.
																		// Only add 1 for the null termination if there actually was text.
																		uint nRealSize = Convert.ToUInt32(nIndex) + 4 + nAdditionalByteCount;
																		oList.dwSize = AdjustSize(oLablChunk.dwSize, nRealSize, oList.dwSize);
																		oLablChunk.dwSize = nRealSize;
																	}

																	oList.aLablInfo.Add(oLablChunk);

																	break;
																}
															case "ltxt":
																{
																	WaveStructs.LtxtChunk oLtxtChunk = new WaveStructs.LtxtChunk();
																	oLtxtChunk.sID                   = sChunkID;
																	oLtxtChunk.dwSize                = oBinaryReader.ReadUInt32();
																	nLastReadBytes                   = 4;
																	nActualSize                      += nLastReadBytes;
																	oLtxtChunk.dwCuePointID          = oBinaryReader.ReadUInt32();
																	nLastReadBytes                   = 4;
																	nActualSize                      += nLastReadBytes;
																	oLtxtChunk.dwSampleLength        = oBinaryReader.ReadUInt32();
																	nLastReadBytes                   = 4;
																	nActualSize                      += nLastReadBytes;
																	oLtxtChunk.dwPurposeID           = oBinaryReader.ReadUInt32();
																	nLastReadBytes                   = 4;
																	nActualSize                      += nLastReadBytes;
																	oLtxtChunk.wCountry              = oBinaryReader.ReadUInt16();
																	nLastReadBytes                   = 2;
																	nActualSize                      += nLastReadBytes;
																	oLtxtChunk.wLanguage             = oBinaryReader.ReadUInt16();
																	nLastReadBytes                   = 2;
																	nActualSize                      += nLastReadBytes;
																	oLtxtChunk.wDialect              = oBinaryReader.ReadUInt16();
																	nLastReadBytes                   = 2;
																	nActualSize                      += nLastReadBytes;
																	oLtxtChunk.wCodePage             = oBinaryReader.ReadUInt16();
																	nLastReadBytes                   = 2;
																	nActualSize                      += nLastReadBytes;

																	// This means there is text.
																	if (oLtxtChunk.dwSize > 20)
																	{
																		// First get the text.
																		string sText = Encoding.Default.GetString(oBinaryReader.ReadBytes(Convert.ToInt32(oLtxtChunk.dwSize) - 20));

																		// Then read a possible padding byte.
																		if ((oLtxtChunk.dwSize % 2) != 0)
																		{
																			nLastReadBytes = 1;
																			nActualSize    += nLastReadBytes;
																			oBinaryReader.ReadByte();
																		}

																		uint nAdditionalByteCount = 0;

																		// The null termination's index.
																		int nIndex = sText.IndexOf('\0', 0);

																		if (nIndex > 0)
																		{
																			nLastReadBytes       = Convert.ToUInt32(nIndex + 1);
																			nActualSize          += nLastReadBytes;
																			oLtxtChunk.sText     = sText.Substring(0, nIndex);
																			nAdditionalByteCount = 1;
																		}
																		else
																		{
																			// This is not a bad thing! I would just like to get informed.
																			Debug.Assert(false, "LtxtChunk without actual text!");
																		}

																		// Here we make sure to adjust the ltxt chunk's size according to what we trimmed.
																		// Only add 1 for the null termination if there actually was text.
																		uint nRealSize = Convert.ToUInt32(nIndex) + 20 + nAdditionalByteCount;
																		oList.dwSize = AdjustSize(oLtxtChunk.dwSize, nRealSize, oList.dwSize);
																		oLtxtChunk.dwSize = nRealSize;
																	}

																	oList.aLtxtInfo.Add(oLtxtChunk);

																	break;
																}
															case "note":
																{
																	WaveStructs.NoteChunk oNoteChunk = new WaveStructs.NoteChunk();
																	oNoteChunk.sID                   = sChunkID;
																	oNoteChunk.dwSize                = oBinaryReader.ReadUInt32();
																	nLastReadBytes                   = 4;
																	nActualSize                      += nLastReadBytes;
																	oNoteChunk.dwCuePointID          = oBinaryReader.ReadUInt32();
																	nLastReadBytes                   = 4;
																	nActualSize                      += nLastReadBytes;

																	// This means there is text.
																	if (oNoteChunk.dwSize > 4)
																	{
																		// First get the text.
																		string sText = Encoding.Default.GetString(oBinaryReader.ReadBytes(Convert.ToInt32(oNoteChunk.dwSize) - 4));

																		// Then read a possible padding byte.
																		if ((oNoteChunk.dwSize % 2) != 0)
																		{
																			nLastReadBytes = 1;
																			nActualSize    += nLastReadBytes;
																			oBinaryReader.ReadByte();
																		}

																		uint nAdditionalByteCount = 0;

																		// The the null termination's index.
																		int nIndex = sText.IndexOf('\0', 0);

																		if (nIndex > 0)
																		{
																			nLastReadBytes       = Convert.ToUInt32(nIndex + 1);
																			nActualSize          += nLastReadBytes;
																			oNoteChunk.sText     = sText.Substring(0, nIndex);
																			nAdditionalByteCount = 1;
																		}
																		else
																		{
																			// This is not a bad thing! I would just like to get informed.
																			Debug.Assert(false, "NoteChunk without actual text!");
																		}

																		// Here we make sure to adjust the note chunk's size according to what we trimmed as well as the LIST chunk's size.
																		// Only add 1 for the null termination if there actually was text.
																		uint nRealSize = Convert.ToUInt32(nIndex) + 4 + nAdditionalByteCount;
																		oList.dwSize = AdjustSize(oNoteChunk.dwSize, nRealSize, oList.dwSize);
																		oNoteChunk.dwSize = nRealSize;
																	}

																	oList.aNoteInfo.Add(oNoteChunk);

																	break;
																}
															default:
																{
																	// If we get here this is currently kinda bad as we do not carry this information over to the new file.
																	// This basically means that when we write out "oList.dwSize" it actually reports a wrong size!
																	// Either we subtract this size from "oList.dwSize" or carry the information along.
																	Debug.Assert(false, "Unsupported adtl-LIST sub-chunk!");

																	uint nSize = oBinaryReader.ReadUInt32();
																	// No need to count size for nActualSize here.
																	long nPos = GetPosition();
																	int nPaddedSize = Convert.ToInt32(nSize);
																	nPaddedSize += ((nPaddedSize % 2) == 0) ? 0 : 1;
																	oList.dwSize -= Convert.ToUInt32(nPaddedSize) + 8; // Subtract padded size + 4 byte ID and 4 byte size.

																	oBinaryReader.BaseStream.Seek(nPos + Convert.ToInt64(nPaddedSize), SeekOrigin.Begin);

																	break;
																}
														}
													}

													break;
												}
											case "INFO":
												{
													// TODO: Create an array for INFO tags in the INFO chunk!
													oList.oInfoChunk = new WaveStructs.InfoChunk();

													while (oBinaryReader.BaseStream.Position < oList.nPosition + Convert.ToInt64(nListChunkSizeForIteration))
													{
														sChunkID       = Encoding.Default.GetString(oBinaryReader.ReadBytes(4));
														nLastReadBytes = 4;
														nActualSize    += nLastReadBytes;
														sChunkID       = sChunkID.ToUpper();
														// Once this data is carried through a project make sure to adjust the size according to the trimming!
														int nSize      = Convert.ToInt32(oBinaryReader.ReadUInt32());
														nLastReadBytes = 4;
														nActualSize    += nLastReadBytes;

														if (nSize > 0)
														{
															string sTrimmed = Encoding.Default.GetString(oBinaryReader.ReadBytes(nSize));
															int nIndex      = sTrimmed.IndexOf('\0', 0);

															if (nIndex >= 0)
															{
																nLastReadBytes = Convert.ToUInt32(nIndex + 1);
																nActualSize    += nLastReadBytes;
																sTrimmed       = sTrimmed.Substring(0, nIndex);

																if ((nSize % 2) != 0)
																{
																	// Read the padding byte.
																	nLastReadBytes = 1;
																	nActualSize    += nLastReadBytes;
																	oBinaryReader.ReadByte();
																}

																switch (sChunkID)
																{
																	case "DISP": // Sound scheme title
																		{
																			string sText = sTrimmed;

																			break;
																		}
																	case "IARL": // Archive location
																		{
																			oList.oInfoChunk.sArchive = sTrimmed;

																			break;
																		}
																	case "IART": // Artist
																		{
																			oList.oInfoChunk.sArtist = sTrimmed;

																			break;
																		}
																	case "IAAR": // Album artist
																		{
																			oList.oInfoChunk.sAlbumArtist = sTrimmed;

																			break;
																		}
																	case "ICMS": // Commissioned
																		{
																			oList.oInfoChunk.sCommissioned = sTrimmed;

																			break;
																		}
																	case "ICMT": // Comments
																		{
																			oList.oInfoChunk.sComments = sTrimmed;

																			break;
																		}
																	case "ICOP": // Copyright information
																		{
																			oList.oInfoChunk.sCopyright = sTrimmed;

																			break;
																		}
																	case "ICRD": // Creation date
																		{
																			oList.oInfoChunk.sCreationDate = sTrimmed;

																			break;
																		}
																	case "IENG": // Engineer, stores the name of the engineer who worked on the file
																		{
																			oList.oInfoChunk.sEngineer = sTrimmed;

																			break;
																		}
																	case "IGNR": // Genre
																		{
																			oList.oInfoChunk.sGenre = sTrimmed;

																			break;
																		}
																	case "IKEY": // Keywords
																		{
																			oList.oInfoChunk.sKeywords = sTrimmed;

																			break;
																		}
																	case "IMED": // Medium
																		{
																			oList.oInfoChunk.sMedium = sTrimmed;

																			break;
																		}
																	case "INAM": // Name, stores the title of the subject of the file
																		{
																			oList.oInfoChunk.sTitleName = sTrimmed;

																			break;
																		}
																	case "IPRD": // Product, specifies the name of the title the file was originally intended for
																		{
																			oList.oInfoChunk.sProduct = sTrimmed;

																			break;
																		}
																	case "ISBJ": // Subject
																		{
																			oList.oInfoChunk.sSubject = sTrimmed;

																			break;
																		}
																	case "ISFT": // Software, identifies the name of the software package used to create the file
																		{
																			oList.oInfoChunk.sSoftware = sTrimmed;

																			break;
																		}
																	case "ISRC": // Source
																		{
																			oList.oInfoChunk.sSource = sTrimmed;

																			break;
																		}
																	case "ITCH": // Technician
																		{
																			oList.oInfoChunk.sTechnician = sTrimmed;

																			break;
																		}
																	case "ITOC": // TableOfContents?
																		{
																			oList.oInfoChunk.sITOC = sTrimmed;

																			break;
																		}
																	case "ITRK": // Track number
																		{
																			oList.oInfoChunk.sTrackNumber = sTrimmed;

																			break;
																		}
																	case "IURL": // URL
																		{
																			oList.oInfoChunk.sURL = sTrimmed;

																			break;
																		}
																	case "TCOD": // Start time code
																		{
																			string start = sTrimmed;

																			break;
																		}
																	case "TCDO": // End time code
																		{
																			string end = sTrimmed;

																			break;
																		}
																	case "JUNQ": // Junk???
																		{
																			Debug.Assert(false, "JUNQ chunk with size!!");

																			break;
																		}
																	default:
																		{
																			Debug.Assert(false, "Unsupported INFO sub-chunk!");

																			break;
																		}
																}
															}
															else
															{
																// This is not a bad thing! I would just like to get informed.
																Debug.Assert(false, "InfoChunk without actual text!");
															}
														}
													}

													break;
												}
										}

										oBinaryReader.BaseStream.Seek(oList.nPosition + Convert.ToInt64(nListChunkSizeForIteration), SeekOrigin.Begin);
									}
									catch (EndOfStreamException e)
									{
										// If we get here it means something is fishy within the LIST chunk. We should therefore keep a correct size instead.
										oList.dwSize = nActualSize - nLastReadBytes;

										string sSignedOrFloat = "unknown size";

										switch (oFormat.wBitsPerSample)
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
													switch (oFormat.wFormatTag)
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

										string sErrorMessage = string.Format("Error reading LIST chunk data! ({0})\nThe below file is badly formatted but can be saved!\n{1}\n\nSample Rate: {2} Hz\nSample Size: {3}\nChannel Count: {4}\nData Format: {5}\n", e.GetType().Name, ((System.IO.FileStream)(oBinaryReader.BaseStream)).Name, oFormat.dwSamplesPerSec, sSignedOrFloat, oFormat.wChannels, oFormat.wFormatTag);
										ShowMessageBox("Badly Formatted Wave File", sErrorMessage);
									}

									aLists.Add(oList);

									break;
								}
							case "cue ":
								{
									oCue.sID         = sChunkID;
									oCue.dwSize      = oBinaryReader.ReadUInt32();
									oCue.nPosition   = GetPosition();
									oCue.dwNumPoints = oBinaryReader.ReadUInt32();

									// Make sure the size is correct.
									Debug.Assert(oCue.dwSize == (4 + oCue.dwNumPoints * 24), "Invalid cue chunk size!");

									for (uint i = 0; i < oCue.dwNumPoints; ++i)
									{
										WaveStructs.CuePoint oCuePoint = new WaveStructs.CuePoint();
										oCuePoint.dwID                 = oBinaryReader.ReadUInt32();
										oCuePoint.dwPosition           = oBinaryReader.ReadUInt32();
										oCuePoint.sDataChunkID         = Encoding.Default.GetString(oBinaryReader.ReadBytes(4));
										oCuePoint.dwChunkStart         = oBinaryReader.ReadUInt32();
										oCuePoint.dwBlockStart         = oBinaryReader.ReadUInt32();
										oCuePoint.dwSampleOffset       = oBinaryReader.ReadUInt32();

										oCue.aCuePoints.Add(oCuePoint);
									}

									oCue.aCuePoints.Sort();

									break;
								}
							default:
								{
									try
									{
										// Add this chunk to the unknown chunks list.
										uint dwChunkSize    = oBinaryReader.ReadUInt32();
										long nChunkPosition = GetPosition();
										aoUnknownChunkInfos.Add(new WaveStructs.ChunkInfo(sChunkID, nChunkPosition, dwChunkSize));

										// And seek forward to the next chunk location
										oBinaryReader.BaseStream.Seek(nChunkPosition + Convert.ToInt64(dwChunkSize), SeekOrigin.Begin);
									}
									catch (EndOfStreamException e)
									{
										string sErrorMessage = "Error reading data! (";
										sErrorMessage        += e.GetType().Name;
										sErrorMessage        += ")\n";
										sErrorMessage        += "The below file is badly formatted!!\n";
										sErrorMessage        += ((System.IO.FileStream)(oBinaryReader.BaseStream)).Name;
										ShowMessageBox("Bad File Stream", sErrorMessage);

										return false;
									}

									break;
								}
						}
					}
					else
					{
						break;
					}
				}
			}

			return true;
		}

		private uint AdjustSize(uint nReportedSize, uint nActualSize, uint nSizeToAdjust)
		{
			if (nReportedSize != nActualSize)
			{
				// Adjust the LIST chunk's size accordingly.
				if (nReportedSize > nActualSize)
				{
					// If the reported size is bigger than the real size subtract.
					nSizeToAdjust -= nReportedSize - nActualSize;

					// If the old size was odd but the new one isn't we must subtract the padding byte.
					if ((nReportedSize % 2) != 0)
					{
						// Old size was odd.
						if ((nActualSize % 2) == 0)
						{
							// New size is now even so subtract a byte.
							nSizeToAdjust -= 1;
						}
					}
					else // If the old size was even and the new one isn't we must add the padding byte.
					{
						// Old size was even.
						if ((nActualSize % 2) != 0)
						{
							// New size is now odd so add a byte.
							nSizeToAdjust += 1;
						}
					}
				}
				else
				{
					// If the reported size is smaller than the real size add.
					nSizeToAdjust += nActualSize - nReportedSize;

					// If the old size was even and the new one isn't we must add the padding byte.
					if ((nReportedSize % 2) == 0)
					{
						// Old size was even.
						if ((nActualSize % 2) != 0)
						{
							// New size is now odd so add a byte.
							nSizeToAdjust += 1;
						}
					}
					else // If the old size was odd but the new one isn't we must subtract the padding byte.
					{
						// Old size was odd.
						if ((nActualSize % 2) == 0)
						{
							// New size is now even so subtract a byte.
							nSizeToAdjust -= 1;
						}
					}
				}
			}

			return nSizeToAdjust;
		}

		public WaveStructs.RiffChunk GetRiffChunk()
		{
			return oRiff;
		}

		public WaveStructs.FormatChunk GetFormatChunk()
		{
			return oFormat;
		}

		public WaveStructs.FactChunk GetFactChunk()
		{
			return oFact;
		}

		public WaveStructs.DataChunk GetDataChunk()
		{
			return oData;
		}

		public WaveStructs.CueChunk GetCueChunk()
		{
			return oCue;
		}

		public ArrayList GetListChunkArray()
		{
			return aLists;
		}

		public void FinalizeFile()
		{
			if (oBinaryReader != null)
			{
				oBinaryReader.Close();
			}

			if (oBinaryWriter != null)
			{
				oBinaryWriter.Close();
			}
		}

		public long GetPosition()
		{
			return oBinaryReader.BaseStream.Position;
		}

		public void WriteWaveHeader(WaveStructs.NewWaveInfo oNewWaveInfo)
		{
			oBinaryWriter.Write(new char[4] { 'R', 'I', 'F', 'F' });
			oBinaryWriter.Write(oNewWaveInfo.dwRiffSize);
			oBinaryWriter.Write(new char[4] { 'W', 'A', 'V', 'E' });
			oBinaryWriter.Write(new char[4] { 'f', 'm', 't', ' ' });
			oBinaryWriter.Write(oNewWaveInfo.dwFormatChunkSize);
			oBinaryWriter.Write(oNewWaveInfo.wFormatTag);
			oBinaryWriter.Write(oNewWaveInfo.wChannels);
			oBinaryWriter.Write(oNewWaveInfo.dwSamplesPerSec);
			oBinaryWriter.Write(oNewWaveInfo.dwAvgBytesPerSec); //(int)(samplerate * ((BitsPerSample * channels) / 8))
			oBinaryWriter.Write(oNewWaveInfo.wBlockAlign);      //(short)((BitsPerSample * channels) / 8)
			oBinaryWriter.Write(oNewWaveInfo.wBitsPerSample);

			if (oNewWaveInfo.wFormatTag == WaveStructs.WAVE_FORMAT_EXTENSIBLE)
			{
				oBinaryWriter.Write(oNewWaveInfo.cbSize);
				oBinaryWriter.Write(oNewWaveInfo.wValidBitsPerSample);
				oBinaryWriter.Write(oNewWaveInfo.dwChannelMask);

				// Make sure the SubFormat is of right type.
				Debug.Assert(oNewWaveInfo.SubFormat == WaveStructs.KSDATAFORMAT_SUBTYPE_PCM, "Mismatching SubFormat for WAVE_FORMAT_EXTENSIBLE!");

				oBinaryWriter.Write(oNewWaveInfo.SubFormat.ToByteArray());
			}
			else if (oNewWaveInfo.wFormatTag == WaveStructs.WAVE_FORMAT_IEEE_FLOAT)
			{
				oBinaryWriter.Write(oNewWaveInfo.cbSize);
			}

			oBinaryWriter.Write(new char[4] { 'd', 'a', 't', 'a' });
			oBinaryWriter.Write(oNewWaveInfo.dwDataSize);
		}

		public byte[] GetCompleteDataChunkDataArray()
		{
			try
			{
				// Make sure to set the reader to the data location.
				oBinaryReader.BaseStream.Seek(oData.nPosition, SeekOrigin.Begin);

				// No padding here! Should be always an even number!?
				byte[] aData = new byte[oData.dwSize];

				// TODO: Have a look at increasing performance here. Unroll loop and use ReadBytes!
				for (uint i = 0; i < oData.dwSize; ++i)
				{
					aData[i] = oBinaryReader.ReadByte();
				}

				return aData;
			}
			catch (EndOfStreamException e)
			{
				string sErrorMessage = "Error reading data! (";
				sErrorMessage        += e.GetType().Name;
				sErrorMessage        += ")";
				ShowMessageBox("Bad File Stream", sErrorMessage);
			}
			catch (OutOfMemoryException e)
			{
				string sErrorMessage = "Error out of memory! (";
				sErrorMessage        += e.GetType().Name;
				sErrorMessage        += ")";
				ShowMessageBox("OOM Situation Occurred", sErrorMessage);
			}

			return null;
		}

		public byte[] GetPartDataChunkDataArray(uint nOffset, uint nSize)
		{
			try
			{
				// Set the read position according to the passed offset and file format.
				oBinaryReader.BaseStream.Seek(oData.nPosition + nOffset, 0);

				byte[] aData = new byte[nSize];

				// TODO: Have a look at increasing performance here. Unroll loop and use ReadBytes!
				for (uint i = 0; i < nSize; ++i)
				{
					aData[i] = oBinaryReader.ReadByte();
				}

				return aData;
			}
			catch (EndOfStreamException e)
			{
				string sErrorMessage = string.Format("Data could not be read from a wave file during splitting procedure!\n\nException Type: {0}\nMessage: {1}\n\nAborting the procedure!", e.GetType().Name, e.Message);
				ShowMessageBox("Reading Data Failed", sErrorMessage);
			}

			return null;
		}

		public bool AppendWaveData(byte[] aData)
		{
			try
			{
				oBinaryWriter.Write(aData);

				return true;
			}
			catch (IOException e)
			{
				string sErrorMessage = string.Format("Data could not be written to the project wave file during merging procedure!\n\nException Type: {0}\nMessage: {1}\n\nAborting the procedure!", e.GetType().Name, e.Message);
				ShowMessageBox("Writing Data Failed", sErrorMessage);
			}

			return false;
		}

		public void WriteAfterWaveData(WaveStructs.NewWaveInfo oNewWaveInfo)
		{
			// Write the "fact" chunk if provided.
			if (oNewWaveInfo.oFactChunk != null)
			{
				oBinaryWriter.Write(Encoding.Default.GetBytes(oNewWaveInfo.oFactChunk.sID));
				oBinaryWriter.Write(oNewWaveInfo.oFactChunk.dwSize);
				oBinaryWriter.Write(oNewWaveInfo.oFactChunk.dwNumSamples);
			}

			// Write the "cue " chunk if provided.
			if (oNewWaveInfo.oCueChunk != null && oNewWaveInfo.oCueChunk.aCuePoints != null)
			{
				oBinaryWriter.Write(Encoding.Default.GetBytes(oNewWaveInfo.oCueChunk.sID));

				// Catch an invalid cue chunk size.
				// We calculate this ourselves in case it was wrong in the source file already or someone/something tampered with the project file.
				uint nSize = 4 + (oNewWaveInfo.oCueChunk.dwNumPoints * 24);
				Debug.Assert(oNewWaveInfo.oCueChunk.dwSize == nSize, "CueChunk size does not match the actual size!");
				oBinaryWriter.Write(nSize);
				oBinaryWriter.Write(oNewWaveInfo.oCueChunk.dwNumPoints);

				foreach (WaveStructs.CuePoint oCuePoint in oNewWaveInfo.oCueChunk.aCuePoints)
				{
					oBinaryWriter.Write(oCuePoint.dwID);
					oBinaryWriter.Write(oCuePoint.dwPosition);
					oBinaryWriter.Write(Encoding.Default.GetBytes(oCuePoint.sDataChunkID));
					oBinaryWriter.Write(oCuePoint.dwChunkStart);
					oBinaryWriter.Write(oCuePoint.dwBlockStart);
					oBinaryWriter.Write(oCuePoint.dwSampleOffset);
				}
			}

			// Write the "LIST" chunks if provided.
			if (oNewWaveInfo.oListChunk != null)
			{
				// Prepared for future LIST chunks that might get supported.
				switch (oNewWaveInfo.oListChunk.sTypeID)
				{
					case "adtl":
						{
							// The  "LIST" chunk header.
							oBinaryWriter.Write(Encoding.Default.GetBytes(oNewWaveInfo.oListChunk.sID));
							oBinaryWriter.Write(oNewWaveInfo.oListChunk.dwSize);
							oBinaryWriter.Write(Encoding.Default.GetBytes(oNewWaveInfo.oListChunk.sTypeID));

							// The "labl" chunks.
							foreach (WaveStructs.LablChunk oLablChunk in oNewWaveInfo.oListChunk.aLablInfo)
							{
								oBinaryWriter.Write(Encoding.Default.GetBytes(oLablChunk.sID));

								// Text length + 1 byte for null termination and + 4 byte for dwCuePointID. Only add 1 for null termination if we actually have text.
								// We calculate this ourselves in case it was wrong in the source file already or someone/something tampered with the project file.
								uint nSize = Convert.ToUInt32(oLablChunk.sText.Length);
								nSize = (nSize > 0) ? nSize + 1 + 4 : 4;
								Debug.Assert(oLablChunk.dwSize == nSize, "LablChunk size does not match the actual size!");
								oLablChunk.dwSize = nSize;
								oBinaryWriter.Write(oLablChunk.dwSize);
								oBinaryWriter.Write(oLablChunk.dwCuePointID);

								// Write the null termination for the string and any padding byte but only if there's actually text.
								if (nSize > 4)
								{
									oBinaryWriter.Write(Encoding.Default.GetBytes(oLablChunk.sText));
									oBinaryWriter.Write(Encoding.Default.GetBytes("\0"));
									
									if ((oLablChunk.dwSize % 2) != 0)
									{
										// Write the padding byte.
										oBinaryWriter.Write(Encoding.Default.GetBytes("\0"));
									}
								}
							}

							// The "ltxt" chunks.
							foreach (WaveStructs.LtxtChunk oLtxtChunk in oNewWaveInfo.oListChunk.aLtxtInfo)
							{
								oBinaryWriter.Write(Encoding.Default.GetBytes(oLtxtChunk.sID));

								// Text length + 1 byte for null termination and + 4 byte for dwCuePointID. Only add 1 for null termination if we actually have text.
								// We calculate this ourselves in case it was wrong in the source file already or someone/something tampered with the project file.
								uint nSize = Convert.ToUInt32(oLtxtChunk.sText.Length);
								nSize = (nSize > 0) ? nSize + 1 + 20 : 20;
								Debug.Assert(oLtxtChunk.dwSize == nSize, "LtxtChunk size does not match the actual size!");
								oLtxtChunk.dwSize = nSize;
								oBinaryWriter.Write(oLtxtChunk.dwSize);
								oBinaryWriter.Write(oLtxtChunk.dwCuePointID);
								oBinaryWriter.Write(oLtxtChunk.dwSampleLength);
								oBinaryWriter.Write(oLtxtChunk.dwPurposeID);
								oBinaryWriter.Write(oLtxtChunk.wCountry);
								oBinaryWriter.Write(oLtxtChunk.wLanguage);
								oBinaryWriter.Write(oLtxtChunk.wDialect);
								oBinaryWriter.Write(oLtxtChunk.wCodePage);

								// Write the null termination for the string and any padding byte but only if there's actually text.
								if (nSize > 20)
								{
									oBinaryWriter.Write(Encoding.Default.GetBytes(oLtxtChunk.sText));
									oBinaryWriter.Write(Encoding.Default.GetBytes("\0"));

									if ((oLtxtChunk.dwSize % 2) != 0)
									{
										// Write the padding byte.
										oBinaryWriter.Write(Encoding.Default.GetBytes("\0"));
									}
								}
							}

							// The "note" chunks.
							foreach (WaveStructs.NoteChunk oNoteChunk in oNewWaveInfo.oListChunk.aNoteInfo)
							{
								oBinaryWriter.Write(Encoding.Default.GetBytes(oNoteChunk.sID));

								// Text length + 1 byte for null termination and + 4 byte for dwCuePointID. Only add 1 for null termination if we actually have text.
								// We calculate this ourselves in case it was wrong in the source file already or someone/something tampered with the project file.
								uint nSize = Convert.ToUInt32(oNoteChunk.sText.Length);
								nSize = (nSize > 0) ? nSize + 1 + 4 : 4;
								Debug.Assert(oNoteChunk.dwSize == nSize, "NoteChunk size does not match the actual size!");
								oNoteChunk.dwSize = nSize;
								oBinaryWriter.Write(oNoteChunk.dwSize);
								oBinaryWriter.Write(oNoteChunk.dwCuePointID);

								// Write the null termination for the string and any padding byte but only if there's actually text.
								if (nSize > 4)
								{
									oBinaryWriter.Write(Encoding.Default.GetBytes(oNoteChunk.sText));
									oBinaryWriter.Write(Encoding.Default.GetBytes("\0"));

									if ((oNoteChunk.dwSize % 2) != 0)
									{
										// Write the padding byte.
										oBinaryWriter.Write(Encoding.Default.GetBytes("\0"));
									}
								}
							}

							break;
						}
					case "INFO":
						{
							// Not yet supported!!

							break;
						}
				}
			}
		}
	}
}
