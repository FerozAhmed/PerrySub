using System;
using System.Collections.Generic;
using System.Text;
using MediaInfoWrapper;
using System.Windows.Forms;
using System.Collections;

namespace scriptASS
{
    partial class mainW
    {
        private bool HasAudio(string fname)
        {
            MediaInfo mi = new MediaInfo(fname);
            return (mi.AudioCount > 0);
        }

        private bool HasVideo(string fname)
        {
            MediaInfo mi = new MediaInfo(fname);
            return (mi.VideoCount > 0);
        }

        private void RetrieveMediaFileInfo(string fname)
        {
            setStatus("Cargando informaci�n de MediaInfo.dll ...");
            MediaInfo mi = new MediaInfo(fname);
            setStatus("Informaci�n actualizada.");

            TreeNode nodo;
            int idx = 1;
            treeView1.Nodes.Clear();

            if (mi.VideoCount > 0)
            {
                TreeNode vidNode;
                nodo = vidNode = treeView1.Nodes.Add("V�deo");
                foreach (VideoTrack vi in mi.Video) 
                {                    
                    nodo = vidNode.Nodes.Add("Stream "+(idx++));
                    nodo.Nodes.Add("C�dec : "+vi.Codec+" ("+vi.CodecInfo+")");
                    nodo.Nodes.Add("Resoluci�n : " + vi.Width+"x"+vi.Height);
                    nodo.Nodes.Add("Frames : " + vi.FrameCount);
                    nodo.Nodes.Add("Fps : " + vi.FrameRateString);
                    nodo.Nodes.Add("Bitrate : " + vi.BitRateString);
                    nodo.Nodes.Add("Duraci�n : " + vi.DurationString);
                    nodo.Nodes.Add("Idioma : " + vi.LanguageString);
                    nodo.Nodes.Add("T�tulo : " + vi.Title);
                    nodo.Nodes.Add("ID : " + vi.ID);
                }
            }
            idx = 1;
            if (mi.AudioCount > 0)
            {
                TreeNode audNode;
                nodo = audNode = treeView1.Nodes.Add("Audio");
                foreach (AudioTrack au in mi.Audio)
                {
                    nodo = audNode.Nodes.Add("Stream "+(idx++));
                    nodo.Nodes.Add("C�dec : " + au.CodecID + " (" + au.CodecIDInfo + ")");
                    nodo.Nodes.Add("Canales : "+au.ChannelsString);
                    nodo.Nodes.Add("Bitrate : " + au.BitRateString + " (" + au.BitRateMode + ")");
                    nodo.Nodes.Add("Samples : " + au.SamplingCount);
                    nodo.Nodes.Add("Sample rate : " + au.SamplingRateString);
                    nodo.Nodes.Add("Duraci�n : "+au.DurationString);
                    nodo.Nodes.Add("Idioma : " + au.LanguageString);      
                    nodo.Nodes.Add("Retraso : "+au.Delay);
                    nodo.Nodes.Add("ID : " + au.ID);
                }
            }
            idx = 1;
            if (mi.TextCount > 0)
            {
                TreeNode subNode;
                nodo = subNode = treeView1.Nodes.Add("Texto");
                foreach (TextTrack tt in mi.Text)
                {
                    nodo = subNode.Nodes.Add("Stream " + (idx++));
                    nodo.Nodes.Add("Formato : " + tt.Codec + " (" + tt.CodecString + ")");
                    nodo.Nodes.Add("Idioma : " + tt.LanguageString);
                    nodo.Nodes.Add("Retraso : " + tt.Delay);
                    nodo.Nodes.Add("ID : " + tt.ID);                    
                }
            }
            treeView1.ExpandAll();
        }

        private ArrayList GetSubtitleTrackID(string fname)
        {
            ArrayList bleh = new ArrayList();
            MediaInfo mi = new MediaInfo(fname);
            if (mi.TextCount > 0)
            {
                foreach (TextTrack tt in mi.Text)
                {
                    switch (tt.CodecString.ToLower())
                    {
                        case "ass":
                        case "utf-8":
                            bleh.Add(tt);
                            break;
                    }
                }
            }
            if (bleh.Count > 0) return bleh;
            
            return null;
        }

    }
}
