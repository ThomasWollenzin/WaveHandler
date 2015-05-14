using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaveHandler
{
	public class WaveStructs
	{
		/* Windows WAVE File Encoding Tags */
		public const ushort WAVE_FORMAT_UNKNOWN		                = 0x0000; /* Unknown Format */
		public const ushort WAVE_FORMAT_PCM			                  = 0x0001; /* PCM */
		public const ushort WAVE_FORMAT_ADPCM		                  = 0x0002; /* Microsoft ADPCM Format */
		public const ushort WAVE_FORMAT_IEEE_FLOAT		            = 0x0003; /* IEEE Float */
		public const ushort WAVE_FORMAT_VSELP		                  = 0x0004; /* Compaq Computer's VSELP */
		public const ushort WAVE_FORMAT_IBM_CSVD		              = 0x0005; /* IBM CVSD */
		public const ushort WAVE_FORMAT_ALAW		                  = 0x0006; /* ALAW */
		public const ushort WAVE_FORMAT_MULAW		                  = 0x0007; /* MULAW */
		public const ushort WAVE_FORMAT_OKI_ADPCM		              = 0x0010; /* OKI ADPCM */
		public const ushort WAVE_FORMAT_DVI_ADPCM		              = 0x0011; /* Intel's DVI ADPCM */
		public const ushort WAVE_FORMAT_MEDIASPACE_ADPCM	        = 0x0012; /* Videologic's MediaSpace ADPCM*/
		public const ushort WAVE_FORMAT_SIERRA_ADPCM	            = 0x0013; /* Sierra ADPCM */
		public const ushort WAVE_FORMAT_G723_ADPCM		            = 0x0014; /* G.723 ADPCM */
		public const ushort WAVE_FORMAT_DIGISTD		                = 0x0015; /* DSP Solution's DIGISTD */
		public const ushort WAVE_FORMAT_DIGIFIX		                = 0x0016; /* DSP Solution's DIGIFIX */
		public const ushort WAVE_FORMAT_DIALOGIC_OKI_ADPCM	      = 0x0017; /* Dialogic OKI ADPCM */
		public const ushort WAVE_FORMAT_MEDIAVISION_ADPCM	        = 0x0018; /* MediaVision ADPCM */
		public const ushort WAVE_FORMAT_CU_CODEC		              = 0x0019; /* HP CU */
		public const ushort WAVE_FORMAT_YAMAHA_ADPCM	            = 0x0020; /* Yamaha ADPCM */
		public const ushort WAVE_FORMAT_SONARC		                = 0x0021; /* Speech Compression's Sonarc */
		public const ushort WAVE_FORMAT_TRUESPEECH		            = 0x0022; /* DSP Group's True Speech */
		public const ushort WAVE_FORMAT_ECHOSC1		                = 0x0023; /* Echo Speech's EchoSC1 */
		public const ushort WAVE_FORMAT_AUDIOFILE_AF36	          = 0x0024; /* Audiofile AF36 */
		public const ushort WAVE_FORMAT_APTX		                  = 0x0025; /* APTX */
		public const ushort WAVE_FORMAT_AUDIOFILE_AF10	          = 0x0026; /* AudioFile AF10 */
		public const ushort WAVE_FORMAT_PROSODY_1612	            = 0x0027; /* Prosody 1612 */
		public const ushort WAVE_FORMAT_LRC			                  = 0x0028; /* LRC */
		public const ushort WAVE_FORMAT_AC2			                  = 0x0030; /* Dolby AC2 */
		public const ushort WAVE_FORMAT_GSM610		                = 0x0031; /* GSM610 */
		public const ushort WAVE_FORMAT_MSNAUDIO		              = 0x0032; /* MSNAudio */
		public const ushort WAVE_FORMAT_ANTEX_ADPCME	            = 0x0033; /* Antex ADPCME */
		public const ushort WAVE_FORMAT_CONTROL_RES_VQLPC	        = 0x0034; /* Control Res VQLPC */
		public const ushort WAVE_FORMAT_DIGIREAL		              = 0x0035; /* Digireal */
		public const ushort WAVE_FORMAT_DIGIADPCM		              = 0x0036; /* DigiADPCM */
		public const ushort WAVE_FORMAT_CONTROL_RES_CR10	        = 0x0037; /* Control Res CR10 */
		public const ushort WAVE_FORMAT_VBXADPCM		              = 0x0038; /* NMS VBXADPCM */
		public const ushort WAVE_FORMAT_ROLAND_RDAC		            = 0x0039; /* Roland RDAC */
		public const ushort WAVE_FORMAT_ECHOSC3		                = 0x003A; /* EchoSC3 */
		public const ushort WAVE_FORMAT_ROCKWELL_ADPCM	          = 0x003B; /* Rockwell ADPCM */
		public const ushort WAVE_FORMAT_ROCKWELL_DIGITALK	        = 0x003C; /* Rockwell Digit LK */
		public const ushort WAVE_FORMAT_XEBEC		                  = 0x003D; /* Xebec */
		public const ushort WAVE_FORMAT_G721_ADPCM		            = 0x0040; /* Antex Electronics G.721 */
		public const ushort WAVE_FORMAT_G728_CELP		              = 0x0041; /* G.728 CELP */
		public const ushort WAVE_FORMAT_MSG723		                = 0x0042; /* MSG723 */
		public const ushort WAVE_FORMAT_MPEG		                  = 0x0050; /* MPEG Layer 1,2 */
		public const ushort WAVE_FORMAT_RT24		                  = 0x0051; /* RT24 */
		public const ushort WAVE_FORMAT_PAC			                  = 0x0051; /* PAC */
		public const ushort WAVE_FORMAT_MPEGLAYER3		            = 0x0055; /* MPEG Layer 3 */
		public const ushort WAVE_FORMAT_CIRRUS		                = 0x0059; /* Cirrus */
		public const ushort WAVE_FORMAT_ESPCM		                  = 0x0061; /* ESPCM */
		public const ushort WAVE_FORMAT_VOXWARE		                = 0x0062; /* Voxware (obsolete) */
		public const ushort WAVE_FORMAT_CANOPUS_ATRAC	            = 0x0063; /* Canopus Atrac */
		public const ushort WAVE_FORMAT_G726_ADPCM		            = 0x0064; /* G.726 ADPCM */
		public const ushort WAVE_FORMAT_G722_ADPCM		            = 0x0065; /* G.722 ADPCM */
		public const ushort WAVE_FORMAT_DSAT		                  = 0x0066; /* DSAT */
		public const ushort WAVE_FORMAT_DSAT_DISPLAY	            = 0x0067; /* DSAT Display */
		public const ushort WAVE_FORMAT_VOXWARE_BYTE_ALIGNED      = 0x0069; /* Voxware Byte Aligned (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_AC8		            = 0x0070; /* Voxware AC8 (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_AC10	            = 0x0071; /* Voxware AC10 (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_AC16	            = 0x0072; /* Voxware AC16 (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_AC20	            = 0x0073; /* Voxware AC20 (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_RT24	            = 0x0074; /* Voxware MetaVoice (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_RT29	            = 0x0075; /* Voxware MetaSound (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_RT29HW	          = 0x0076; /* Voxware RT29HW (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_VR12	            = 0x0077; /* Voxware VR12 (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_VR18	            = 0x0078; /* Voxware VR18 (obsolete) */
		public const ushort WAVE_FORMAT_VOXWARE_TQ40	            = 0x0079; /* Voxware TQ40 (obsolete) */
		public const ushort WAVE_FORMAT_SOFTSOUND		              = 0x0080; /* Softsound */
		public const ushort WAVE_FORMAT_VOXWARE_TQ60	            = 0x0081; /* Voxware TQ60 (obsolete) */
		public const ushort WAVE_FORMAT_MSRT24		                = 0x0082; /* MSRT24 */
		public const ushort WAVE_FORMAT_G729A		                  = 0x0083; /* G.729A */
		public const ushort WAVE_FORMAT_MVI_MV12		              = 0x0084; /* MVI MV12 */
		public const ushort WAVE_FORMAT_DF_G726		                = 0x0085; /* DF G.726 */
		public const ushort WAVE_FORMAT_DF_GSM610		              = 0x0086; /* DF GSM610 */
		public const ushort WAVE_FORMAT_ONLIVE		                = 0x0089; /* Onlive */
		public const ushort WAVE_FORMAT_SBC24		                  = 0x0091; /* SBC24 */
		public const ushort WAVE_FORMAT_DOLBY_AC3_SPDIF	          = 0x0092; /* Dolby AC3 SPDIF */
		public const ushort WAVE_FORMAT_ZYXEL_ADPCM		            = 0x0097; /* ZyXEL ADPCM */
		public const ushort WAVE_FORMAT_PHILIPS_LPCBB	            = 0x0098; /* Philips LPCBB */
		public const ushort WAVE_FORMAT_PACKED		                = 0x0099; /* Packed */
		public const ushort WAVE_FORMAT_RHETOREX_ADPCM	          = 0x0100; /* Rhetorex ADPCM */
		public const ushort WAVE_FORMAT_IRAT		                  = 0x0101; /* BeCubed Software's IRAT */
		public const ushort WAVE_FORMAT_VIVO_G723		              = 0x0111; /* Vivo G.723 */
		public const ushort WAVE_FORMAT_VIVO_SIREN		            = 0x0112; /* Vivo Siren */
		public const ushort WAVE_FORMAT_DIGITAL_G723	            = 0x0123; /* Digital G.723 */
		public const ushort WAVE_FORMAT_CREATIVE_ADPCM	          = 0x0200; /* Creative ADPCM */
		public const ushort WAVE_FORMAT_CREATIVE_FASTSPEECH8      = 0x0202; /* Creative FastSpeech8 */
		public const ushort WAVE_FORMAT_CREATIVE_FASTSPEECH10     = 0x0203; /* Creative FastSpeech10 */
		public const ushort WAVE_FORMAT_QUARTERDECK		            = 0x0220; /* Quarterdeck */
		public const ushort WAVE_FORMAT_FM_TOWNS_SND	            = 0x0300; /* FM Towns Snd */
		public const ushort WAVE_FORMAT_BTV_DIGITAL		            = 0x0400; /* BTV Digital */
		public const ushort WAVE_FORMAT_VME_VMPCM		              = 0x0680; /* VME VMPCM */
		public const ushort WAVE_FORMAT_OLIGSM		                = 0x1000; /* OLIGSM */
		public const ushort WAVE_FORMAT_OLIADPCM		              = 0x1001; /* OLIADPCM */
		public const ushort WAVE_FORMAT_OLICELP		                = 0x1002; /* OLICELP */
		public const ushort WAVE_FORMAT_OLISBC		                = 0x1003; /* OLISBC */
		public const ushort WAVE_FORMAT_OLIOPR		                = 0x1004; /* OLIOPR */
		public const ushort WAVE_FORMAT_LH_CODEC		              = 0x1100; /* LH Codec */
		public const ushort WAVE_FORMAT_NORRIS		                = 0x1400; /* Norris */
		//?public const ushort WAVE_FORMAT_ISIAUDIO               = 0x0088; /* ISIAudio */
		//?public const ushort WAVE_FORMAT_ISIAUDIO		            = 0x1401; /* ISIAudio */
		public const ushort WAVE_FORMAT_SOUNDSPACE_MUSICOMPRESS   = 0x1500; /* Soundspace Music Compression */
		public const ushort WAVE_FORMAT_DVM			                  = 0x2000; /* DVM */
		public const ushort WAVE_FORMAT_EXTENSIBLE		            = 0xFFFE; /* SubFormat */
		public const ushort WAVE_FORMAT_DEVELOPMENT               = 0xFFFF; /* Development */
		public const ushort WAVE_FORMAT_IBM_MULAW	                = 0x0101; /*  IBM MULAW */
		public const ushort WAVE_FORMAT_IBM_ALAW		              = 0x0102; /*IBM ALAW */
		public const ushort WAVE_FORMAT_IBM_ADPCM	                = 0x0103; /* IBM ADPCM */
		public const ushort WAVE_FORMAT_DIVX_AUDIO160	            = 0x00000160; /* DivX Audio */
		public const ushort WAVE_FORMAT_DIVX_AUDIO161	            = 0x00000161; /* DivX Audio */

		// Sub formats
		public static Guid KSDATAFORMAT_SUBTYPE_PCM                         = new Guid("00000001-0000-0010-8000-00aa00389b71"); // PCM audio.
		public static Guid KSDATAFORMAT_SUBTYPE_ADPCM                       = new Guid("00000002-0000-0010-8000-00aa00389b71"); // Adaptive delta pulse code modulation (ADPCM).
		public static Guid KSDATAFORMAT_SUBTYPE_IEEE_FLOAT                  = new Guid("00000003-0000-0010-8000-00aa00389b71"); // IEEE floating-point audio.
		public static Guid KSDATAFORMAT_SUBTYPE_ALAW                        = new Guid("00000006-0000-0010-8000-00aa00389b71"); // A-law coding.
		public static Guid KSDATAFORMAT_SUBTYPE_MULAW                       = new Guid("00000007-0000-0010-8000-00aa00389b71"); // μ-law coding.
		public static Guid KSDATAFORMAT_SUBTYPE_DRM                         = new Guid("00000009-0000-0010-8000-00aa00389b71"); // DRM-encoded format for digital-audio content protected by Microsoft Digital Rights Management.
		public static Guid KSDATAFORMAT_SUBTYPE_IEC61937_DOLBY_DIGITAL_PLUS = new Guid("0000000a-0cea-0010-8000-00aa00389b71"); // Dolby Digital Plus formatted for HDMI output.
		public static Guid KSDATAFORMAT_SUBTYPE_IEC61937_DOLBY_DIGITAL      = new Guid("00000092-0000-0010-8000-00aa00389b71"); // Dolby Digital Plus formatted for S/PDIF or HDMI output.
		public static Guid KSDATAFORMAT_SUBTYPE_MPEG                        = new Guid("00000050-0000-0010-8000-00aa00389b71"); // MPEG-1 audio payload.
		
		// Speaker position
		public const uint SPEAKER_FRONT_LEFT            = 0x1;
		public const uint SPEAKER_FRONT_RIGHT           = 0x2;
		public const uint SPEAKER_FRONT_CENTER          = 0x4;
		public const uint SPEAKER_LOW_FREQUENCY         = 0x8;
		public const uint SPEAKER_BACK_LEFT             = 0x10;
		public const uint SPEAKER_BACK_RIGHT            = 0x20;
		public const uint SPEAKER_FRONT_LEFT_OF_CENTER  = 0x40;
		public const uint SPEAKER_FRONT_RIGHT_OF_CENTER = 0x80;
		public const uint SPEAKER_BACK_CENTER           = 0x100;
		public const uint SPEAKER_SIDE_LEFT             = 0x200;
		public const uint SPEAKER_SIDE_RIGHT            = 0x400;
		public const uint SPEAKER_TOP_CENTER            = 0x800;
		public const uint SPEAKER_TOP_FRONT_LEFT        = 0x1000;
		public const uint SPEAKER_TOP_FRONT_CENTER      = 0x2000;
		public const uint SPEAKER_TOP_FRONT_RIGHT       = 0x4000;
		public const uint SPEAKER_TOP_BACK_LEFT         = 0x8000;
		public const uint SPEAKER_TOP_BACK_CENTER       = 0x10000;
		public const uint SPEAKER_TOP_BACK_RIGHT        = 0x20000;

		/// <summary>
		/// For retrieving general wave file information
		/// </summary>
		public class RiffChunk
		{
			public RiffChunk()
			{
				sID = String.Empty;
				dwSize = 0;
				sFormat = String.Empty;
				nPosition = 0;
			}

			public string sID;                 // RIFF
			public uint dwSize;                // Chunk size
			public string sFormat;             // Must be WAVE
			public long nPosition;             // Position in file
		}

		/// <summary>
		/// For retrieving the wave file format description
		/// </summary>
		public class FormatChunk
		{
			public string sID;    	           // Must be "fmt "
			public uint dwSize;                // Sub-chunk size

			public long nPosition;             // Position in file

			// Data for sample rates of 8, 16, 24 and 32 bits per sample
			public ushort wFormatTag;  	       // If uncompressed 1 = PCM
			public ushort wChannels;           // Mono = 1, Stereo = 2 etc.
			public uint dwSamplesPerSec;       // Sample rate 44100, 48000 Hz etc.
			public uint dwAvgBytesPerSec;      // SampleRate * NumChannels * BitsPerSample/8
			public ushort wBlockAlign;         // NumChannels * BitsPerSample/8
			public ushort wBitsPerSample;      // 8 bits = 8, 16 bits = 16, etc

			// Data for sample rates of 24 and 32 bits per sample
			public ushort cbSize;
			public ushort wValidBitsPerSample; // Bits of precision
			public uint dwChannelMask;         // Which channels are present in stream
			public Guid SubFormat;
		}

		/// <summary>
		/// For retrieving the compression ratio in case the wave file is compressed
		/// </summary>
		public class FactChunk
		{
      public FactChunk(){}

      public FactChunk(FactChunk oFactChunkToCopy)
      {
        sID          = oFactChunkToCopy.sID;
        dwSize       = oFactChunkToCopy.dwSize;
        dwNumSamples = oFactChunkToCopy.dwNumSamples;
        nPosition    = oFactChunkToCopy.nPosition;
      }

			public string sID;    		         // Must be "fact"
			public uint dwSize;    	           // Sub-chunk size
			public uint dwNumSamples;          // Number of audio frames
			public long nPosition;             // Position in file
		}

		/// <summary>
		/// For retrieving the actual wave file data
		/// </summary>
		public class DataChunk
		{
			public string sID;    		         // Must be "data"
			public uint dwSize;    	           // Sub-chunk size
			public long nPosition;             // Position in file

			//The following non-standard fields were created to simplify
			//editing.  We need to know, for filestream seeking purposes,
			//the beginning file position of the data chunk.  It's useful to
			//hold the number of samples in the data chunk itself.  Finally,
			//the minute and second length of the file are useful to output
			//to XML.
			//public long lFilePosition;	//Position of data chunk in file
			//public uint dwMinLength;		//Length of audio in minutes
			//public double dSecLength;		//Length of audio in seconds
			//public uint dwNumSamples;		//Number of audio frames
			//Different arrays for the different frame sizes
			//public byte  [] byteArray; 	//8 bit - unsigned
			//public short [] shortArray;    //16 bit - signed
		}

		public class CueChunk
		{
			public CueChunk()
			{
				aCuePoints = new ArrayList();
			}

      public CueChunk(CueChunk oCueChunkToCopy)
      {
        sID         = oCueChunkToCopy.sID;
        dwSize      = oCueChunkToCopy.dwSize;
        dwNumPoints = oCueChunkToCopy.dwNumPoints;
        oCueChunkToCopy.aCuePoints.CopyTo(aCuePoints.ToArray());
        nPosition   = oCueChunkToCopy.nPosition;
      }

			public string sID;    		         // Must be "cue "
			public uint dwSize;    					   // Depends on cue point count
			public uint dwNumPoints;				   // Number if embedded cue points
			public ArrayList aCuePoints;		   // Array of found cue points
			public long nPosition;					   // Position in file
		}

		public class CuePoint
		{
			public uint dwID;    						   // Unique identifier
			public uint dwPosition;					   // Play position
			public string sDataChunkID;			   // Specifies the chunk ID of the Data or Wave List chunk which actually contains the waveform data to which this CuePoint refers
			public uint dwChunkStart;				   // Specifies the byte offset of the start of the 'data' or 'slnt' chunk which actually contains the waveform data to which this CuePoint refers
			public uint dwBlockStart;				   // Specifies the byte offset of the start of the block containing the position
			public uint dwSampleOffset;			   // Specifies the sample offset of the cue point relative to the start of the block
		}

		public class ListChunk
		{
      public ListChunk()
			{
				aLablInfo = new ArrayList();
				aLtxtInfo = new ArrayList();
				aNoteInfo = new ArrayList();
			}

      public ListChunk(ListChunk oListChunkToCopy)
      {
        sID     = oListChunkToCopy.sID;
        dwSize  = oListChunkToCopy.dwSize;
        sTypeID = oListChunkToCopy.sTypeID;
        oListChunkToCopy.aLablInfo.CopyTo(aLablInfo.ToArray());
        oListChunkToCopy.aLtxtInfo.CopyTo(aLtxtInfo.ToArray());
        oListChunkToCopy.aNoteInfo.CopyTo(aNoteInfo.ToArray());

        // Copy the info chunk if it's valid.
        if (oListChunkToCopy.oInfoChunk != null)
        {
          oInfoChunk = new InfoChunk(oListChunkToCopy.oInfoChunk);
        }
        else
        {
          oInfoChunk = null;
        }

        nPosition = oListChunkToCopy.nPosition;
      }

			public string sID;    					   // Must be "LIST".
			public uint dwSize;    					   // Chunk + sub-chunks' size.
			public string sTypeID;					   // Must be "adt1".
			public ArrayList aLablInfo;        // Array of found "labl" chunks.
			public ArrayList aLtxtInfo; 	     // Array of found "ltxt" chunks.
			public ArrayList aNoteInfo; 	     // Array of found "note" chunks.
			public InfoChunk oInfoChunk;       // Holds data from the "INFO" chunk.
			public long nPosition;					   // Position in file.
		}

		public class LablChunk
		{
			public string sID;    		         // Must be "labl"
			public uint dwSize;    	           // Depends on contained text
			public uint dwCuePointID;          // 0 - 0xFFFFFFFF
			public string sText;               // User supplied text
		}

		public class LtxtChunk
		{
			public string sID;    		         // Must be "ltxt"
			public uint dwSize;    	           // Depends on contained text
			public uint dwCuePointID;          // 0 - 0xFFFFFFFF
			public uint dwSampleLength;        // 0 - 0xFFFFFFFF
			public uint dwPurposeID;           // 0 - 0xFFFFFFFF
			public ushort wCountry;            // 0 - 0xFFFF
			public ushort wLanguage;           // 0 - 0xFFFF
			public ushort wDialect;            // 0 - 0xFFFF
			public ushort wCodePage;           // 0 - 0xFFFF
			public string sText;               // User supplied text
		}

		public class NoteChunk
		{
			public string sID;    		         // Must be "note".
			public uint dwSize;    	           // Depends on contained text.
			public uint dwCuePointID;          // 0 - 0xFFFFFFFF.
			public string sText;               // User supplied text.
		}

		public class InfoChunk
		{
      public InfoChunk(){}

      public InfoChunk(InfoChunk oInfoChunkToCopy)
      {
        sArchive      = oInfoChunkToCopy.sArchive;
        sArtist       = oInfoChunkToCopy.sArtist;
        sCommissioned = oInfoChunkToCopy.sCommissioned;
        sComments     = oInfoChunkToCopy.sComments;
        sCopyright    = oInfoChunkToCopy.sCopyright;
        sCreationDate = oInfoChunkToCopy.sCreationDate;
        sEngineer     = oInfoChunkToCopy.sEngineer;
        sGenre        = oInfoChunkToCopy.sGenre;
        sKeywords     = oInfoChunkToCopy.sKeywords;
        sMedium       = oInfoChunkToCopy.sMedium;
        sTitleName    = oInfoChunkToCopy.sTitleName;
        sProduct      = oInfoChunkToCopy.sProduct;
        sSubject      = oInfoChunkToCopy.sSubject;
        sSoftware     = oInfoChunkToCopy.sSoftware;
        sSource       = oInfoChunkToCopy.sSource;
        sTechnician   = oInfoChunkToCopy.sTechnician;
        sITOC         = oInfoChunkToCopy.sITOC;
        sTrackNumber  = oInfoChunkToCopy.sTrackNumber;
        sURL          = oInfoChunkToCopy.sURL;
        sAlbumArtist  = oInfoChunkToCopy.sAlbumArtist;
      }

      public string sArchive      { get; set; } // "IARL": archival location, indicates where the subject of the file is archived
			public string sArtist       { get; set; } // "IART": lists the artist of the original subject of the file
      public string sCommissioned { get; set; } // "ICMS": commissioned, lists the name of the person or organization that commissioned the subject of the file, for example "Pope Julian II"
      public string sComments     { get; set; }	// "ICMT": comments
      public string sCopyright    { get; set; } // "ICOP": copyright information
      public string sCreationDate { get; set; } // "ICRD": creation date
      public string sEngineer     { get; set; } // "IENG": stores the name of the engineer who worked on the file
      public string sGenre        { get; set; } // "IGNR": genre
      public string sKeywords     { get; set; } // "IKEY": keywords
      public string sMedium       { get; set; } // "IMED": medium, describes the original subject of the file, such as "computer image", "drawing", "lithograph" and so forth
      public string sTitleName    { get; set; } // "INAM": stores the title of the subject of the file
      public string sProduct      { get; set; } // "IPRD": product, specifies the name of the title the file was originally intended for
      public string sSubject      { get; set; } // "ISBJ": subject, describes the subject of the file, such as "Aerial view of Seattle"
      public string sSoftware     { get; set; } // "ISFT": software, identifies the name of the software package used to create the file
      public string sSource       { get; set; } // "ISRC": source, identifies the name of the person or organization who supplied the original subject of the file, for example "Trey Research"
      public string sTechnician   { get; set; } // "ITCH": technician, identifies the technician who digitized the subject file, for example "Smith, John"
      public string sITOC         { get; set; } // "ITOC": TableOfContents?
      public string sTrackNumber  { get; set; } // "ITRK": track number
      public string sURL          { get; set; } // "IURL": URL
      public string sAlbumArtist  { get; set; } // "IAAR": Album artist

			// Other non-wave related INFO sub-chunks are
			// "ICRP": cropped, describes whether an image has been cropped and, if so, how it was cropped
			// "IDIM": dimensions, specifies the size of the original subject of the file
			// "IDPI": dots per inch, stores dots per inch setting of the digitizer used to produce the file
			// "ILGT": lightness, describes the changes in lightness settings on the digitizer required to produce the file. Note that the format of this information depends on hardware used
			// "IPLT": palette setting, specifies the number of colors requested when digitizing an image, such as "256"
			// "ISHP": sharpness, identifies the changes in sharpness for the digitizer required to produce the file (the format depends on the hardware used)
			// "ISRF": source form, identifies the original form of the material that was digitized such as "slide", "paper", "map" and so forth, this is not necessarily the same as IMED
		}

		public class NewWaveInfo
		{
      public NewWaveInfo(){}

      public NewWaveInfo(NewWaveInfo oNewWaveInfoToCopy)
      {
        dwRiffSize          = oNewWaveInfoToCopy.dwRiffSize;
        wFormatTag          = oNewWaveInfoToCopy.wFormatTag;
        wChannels           = oNewWaveInfoToCopy.wChannels;
        dwSamplesPerSec     = oNewWaveInfoToCopy.dwSamplesPerSec;
        dwAvgBytesPerSec    = oNewWaveInfoToCopy.dwAvgBytesPerSec;
        wBlockAlign         = oNewWaveInfoToCopy.wBlockAlign;
        wBitsPerSample      = oNewWaveInfoToCopy.wBitsPerSample;
        dwDataSize          = oNewWaveInfoToCopy.dwDataSize;
        dwFormatChunkSize   = oNewWaveInfoToCopy.dwFormatChunkSize;
        cbSize              = oNewWaveInfoToCopy.cbSize;
        wValidBitsPerSample = oNewWaveInfoToCopy.wValidBitsPerSample;
        dwChannelMask       = oNewWaveInfoToCopy.dwChannelMask;
        SubFormat           = oNewWaveInfoToCopy.SubFormat;

        // Copy the fact chunk if it's valid.
        if (oNewWaveInfoToCopy.oFactChunk != null)
        {
          oFactChunk = new FactChunk(oNewWaveInfoToCopy.oFactChunk);
        }
        else
        {
          oFactChunk = null;
        }

        // Copy the cue chunk if it's valid.
        if (oNewWaveInfoToCopy.oCueChunk != null)
        {
          oCueChunk = new CueChunk(oNewWaveInfoToCopy.oCueChunk);
        }
        else
        {
          oCueChunk = null;
        }

        // Copy the list chunk if it's valid.
        if (oNewWaveInfoToCopy.oListChunk != null)
        {
          oListChunk = new ListChunk(oNewWaveInfoToCopy.oListChunk);
        }
        else
        {
          oListChunk = null;
        }
      }

			public uint dwRiffSize             {get; set;} // Entire file size
			public ushort wFormatTag   	       {get; set;} // If uncompressed 1 = PCM
			public ushort wChannels            {get; set;} // Mono = 1, Stereo = 2 etc.
			public uint dwSamplesPerSec        {get; set;} // Sample rate 44100, 48000 Hz etc.
			public uint dwAvgBytesPerSec       {get; set;} // SampleRate * NumChannels * BitsPerSample/8
			public ushort wBlockAlign          {get; set;} // NumChannels * BitsPerSample/8
			public ushort wBitsPerSample       {get; set;} // 8 bits = 8, 16 bits = 16, etc.
			//public uint dwNumSamples           {get; set;} // Number of audio frames
			public uint dwDataSize             {get; set;} // Size of the new data
			public uint dwFormatChunkSize      {get; set;} // Sub-chunk size

			// Data for sample rates of 24 and 32 bits per sample
			public ushort cbSize               {get; set;}
			public ushort wValidBitsPerSample  {get; set;} // Bits of precision.
			public uint dwChannelMask          {get; set;} // Which channels are present in stream.
			public Guid SubFormat              {get; set;} // Guid subformat as described in WaveStructs.cs.
			public FactChunk oFactChunk        {get; set;} // The "fact" chunk.
			public CueChunk oCueChunk          {get; set;} // The "cue" chunk.
			public ListChunk oListChunk        {get; set;} // List chunk with an array of found "labl" chunks.
		}

		public class ChunkInfo
		{
			public ChunkInfo(string _sID, long _nPosition, uint _nSize)
			{
				sID = _sID;
				nPosition = _nPosition;
				nSize = _nSize;
			}

			public string sID;
			public long nPosition;
			public uint nSize;
		}
	}
}
