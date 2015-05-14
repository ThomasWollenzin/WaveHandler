using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WaveHandler
{
	public partial class Form3 : Form
	{
		WaveStructs.NewWaveInfo g_oMergedWaveSettings = new WaveStructs.NewWaveInfo();

		public Form3()
		{
			InitializeComponent();
		}

		public void SetValues(WaveStructs.NewWaveInfo oWaveInfo)
		{
			g_oMergedWaveSettings = oWaveInfo;
			comboBox1.Text = g_oMergedWaveSettings.dwSamplesPerSec.ToString();
			comboBox3.Text = g_oMergedWaveSettings.wChannels.ToString();

			switch (g_oMergedWaveSettings.wBitsPerSample)
			{
				case 8:
					{
						comboBox2.Text = "PCM unsigned 8 bit";

						break;
					}
				case 16:
					{
						comboBox2.Text = "PCM signed 16 bit";

						break;
					}
				case 24:
					{
						comboBox2.Text = "PCM signed 24 bit";

						break;
					}
				case 32:
					{
						switch (g_oMergedWaveSettings.wFormatTag)
						{
							case WaveStructs.WAVE_FORMAT_EXTENSIBLE:
								{
									comboBox2.Text = "PCM signed 32 bit";
									
									break;
								}
								case WaveStructs.WAVE_FORMAT_IEEE_FLOAT:
								{
									comboBox2.Text = "IEEE float signed 32 bit";
									
									break;
								}
						}

						break;
					}
			}
		}

		public WaveStructs.NewWaveInfo GetSettings()
		{
			return g_oMergedWaveSettings;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			g_oMergedWaveSettings.dwSamplesPerSec  = Convert.ToUInt32(comboBox1.Text);
			g_oMergedWaveSettings.wChannels        = Convert.ToUInt16(comboBox3.Text);

			switch (comboBox2.Text)
			{
				case "PCM unsigned 8 bit":
					{
						// Uncompressed wave file.
						g_oMergedWaveSettings.wFormatTag          = WaveStructs.WAVE_FORMAT_PCM;
						g_oMergedWaveSettings.dwFormatChunkSize   = 16;
						g_oMergedWaveSettings.cbSize              = 0;
						g_oMergedWaveSettings.wValidBitsPerSample = 0;
						g_oMergedWaveSettings.dwChannelMask       = 0;
						g_oMergedWaveSettings.wBitsPerSample      = 8;
						break;
					}
				case "PCM signed 16 bit":
					{
						// Uncompressed, canonical WAVE file.
						g_oMergedWaveSettings.wFormatTag          = WaveStructs.WAVE_FORMAT_PCM;
						g_oMergedWaveSettings.dwFormatChunkSize   = 16;
						g_oMergedWaveSettings.cbSize              = 0;
						g_oMergedWaveSettings.wValidBitsPerSample = 0;
						g_oMergedWaveSettings.dwChannelMask       = 0;
						g_oMergedWaveSettings.wBitsPerSample      = 16;

						break;
					}
				case "PCM signed 24 bit":
					{
						// WAVE_FORMAT_EXTENSIBLE
						g_oMergedWaveSettings.wFormatTag          = WaveStructs.WAVE_FORMAT_EXTENSIBLE;
						g_oMergedWaveSettings.dwFormatChunkSize   = 40;
						g_oMergedWaveSettings.cbSize              = 22;
						g_oMergedWaveSettings.wValidBitsPerSample = 24;
						g_oMergedWaveSettings.wBitsPerSample      = 24;

						switch (g_oMergedWaveSettings.wChannels)
						{
							case 1:
								{
									// Mono
									g_oMergedWaveSettings.dwChannelMask = 0x00000000;

									break;
								}
							case 2:
								{
									// Stereo
									g_oMergedWaveSettings.dwChannelMask = WaveStructs.SPEAKER_FRONT_LEFT | WaveStructs.SPEAKER_FRONT_RIGHT;

									break;
								}
						}

						g_oMergedWaveSettings.SubFormat = WaveStructs.KSDATAFORMAT_SUBTYPE_PCM;

						break;
					}
				case "PCM signed 32 bit":
					{
						g_oMergedWaveSettings.wFormatTag          = WaveStructs.WAVE_FORMAT_EXTENSIBLE;
						g_oMergedWaveSettings.dwFormatChunkSize   = 40;
						g_oMergedWaveSettings.cbSize              = 22;
						g_oMergedWaveSettings.wValidBitsPerSample = 32;
						g_oMergedWaveSettings.wBitsPerSample      = 32;

						switch (g_oMergedWaveSettings.wChannels)
						{
							case 1:
								{
									// Mono
									g_oMergedWaveSettings.dwChannelMask = 0x00000000;

									break;
								}
							case 2:
								{
									// Stereo
									g_oMergedWaveSettings.dwChannelMask = WaveStructs.SPEAKER_FRONT_LEFT | WaveStructs.SPEAKER_FRONT_RIGHT;

									break;
								}
						}

						g_oMergedWaveSettings.SubFormat = WaveStructs.KSDATAFORMAT_SUBTYPE_PCM;

						break;
					}
				case "IEEE float signed 32 bit":
					{
						g_oMergedWaveSettings.wFormatTag          = WaveStructs.WAVE_FORMAT_IEEE_FLOAT;
						g_oMergedWaveSettings.dwFormatChunkSize   = 18;
						g_oMergedWaveSettings.cbSize              = 0;
						g_oMergedWaveSettings.wValidBitsPerSample = 32;
						g_oMergedWaveSettings.wBitsPerSample      = 32;

						switch (g_oMergedWaveSettings.wChannels)
						{
							case 1:
								{
									// Mono
									g_oMergedWaveSettings.dwChannelMask = 0x00000000;

									break;
								}
							case 2:
								{
									// Stereo
									g_oMergedWaveSettings.dwChannelMask = WaveStructs.SPEAKER_FRONT_LEFT | WaveStructs.SPEAKER_FRONT_RIGHT;

									break;
								}
						}

						g_oMergedWaveSettings.SubFormat = WaveStructs.KSDATAFORMAT_SUBTYPE_IEEE_FLOAT;

						break;
					}
			}

			g_oMergedWaveSettings.dwAvgBytesPerSec = Convert.ToUInt32(g_oMergedWaveSettings.dwSamplesPerSec * ((g_oMergedWaveSettings.wBitsPerSample * g_oMergedWaveSettings.wChannels) / 8));
			g_oMergedWaveSettings.wBlockAlign      = Convert.ToUInt16(g_oMergedWaveSettings.wChannels * g_oMergedWaveSettings.wBitsPerSample / 8);

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
