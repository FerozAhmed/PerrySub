using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace scriptASS
{
    partial class mainW
    {
        #region VIDEO TIMING (MEMBSUB)

        private void MembSub_markBegin()
        {
            if (gridASS.SelectedRows.Count < 1) return;

            if (VideoState != ReproductionState.Play)
            {
                VideoState = ReproductionState.Play;
                mediaControl.Run();
            }

            double actual = (double)seekBar.Value / videoInfo.FrameRate;
            string s_act = Tiempo.SecondToTimeString(actual);
            setStatus("[MembSub] Marcado frame " + FrameIndex + " como INICIO de l�nea [" + s_act + "]");

            textInicio.Text = s_act;
            framesInicio.Text = FrameIndex.ToString();
            commitChanges();
        }

        private void MembSub_markEnd()
        {
            if (gridASS.SelectedRows.Count < 1) return;
            double actual = (double)seekBar.Value / videoInfo.FrameRate;
            string s_act = Tiempo.SecondToTimeString(actual);
            setStatus("[MembSub] Marcado frame " + FrameIndex + " como FINAL de l�nea [" + s_act + "]");

            string nFinalTiempo = s_act;
            string nFinalFrame = FrameIndex.ToString();

            textFin.Text = nFinalTiempo;
            framesFin.Text = nFinalFrame;

            commitChanges();            

            int idx = gridASS.SelectedRows[0].Index;

            if (idx < gridASS.Rows.Count - 1) // actualizamos seleccion y sus cosas
            {
                gridASS.Rows[idx].Selected = false;
                gridASS.Rows[idx + 1].Selected = true;
                framesInicio.Text = nFinalFrame;
                textInicio.Text = nFinalTiempo;
                moveViewRows(idx);
            }
            
        }

        private void MembSub_playPause()
        {
            if ((VideoState == ReproductionState.Pause) || (VideoState == ReproductionState.Stop))
            {
                mediaControl.Run();
                VideoState = ReproductionState.Play;
                setStatus("[MembSub] Control de V�deo: Play");
            }
            else
            {
                VideoState = ReproductionState.Pause;
                mediaControl.Stop();
                setStatus("[MembSub] Control de V�deo: Pause");
            }
        }

        void MembSubHandling(object sender, KeyEventArgs e)
        {
            if (!sincronizarDeV�deoMembSubToolStripMenuItem.Checked) return;
            switch (e.KeyCode)
            {
                case Keys.B:
                    MembSub_markBegin();
                    break;

                case Keys.E:
                case Keys.N:
                    MembSub_markEnd();
                    break;

                case Keys.Space:
                case Keys.V:
                    MembSub_playPause();
                    break;

            }
        }

        private void membSubMarcarInicioDeL�neaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MembSub_markBegin();
        }

        private void membSubMarcarFinalDeL�neaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MembSub_markEnd();
        }

        private void membSubPlayPauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MembSub_playPause();
        }

        private void ActivateMembSub()
        {
            // activo
            if (sincronizarDeV�deoMembSubToolStripMenuItem.Checked)
            {
                setStatus("MembSub activado");
                updateMenuEnables();
                gridASS.RowsDefaultCellStyle.SelectionBackColor = Color.Crimson;
                gridASS.MultiSelect = false;
                gridASS.Focus();
            }
            else // no activo
            {
                setStatus("MembSub desactivado");
                updateMenuEnables();
            }

            if (gridASS.Rows.Count > 0)
                gridASS.Rows[0].Selected = true;
        }

        private void sincronizarDeV�deoMembSubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sincronizarDeV�deoMembSubToolStripMenuItem.Checked = !sincronizarDeV�deoMembSubToolStripMenuItem.Checked;
            videoMemb.Checked = sincronizarDeV�deoMembSubToolStripMenuItem.Checked;

            ActivateMembSub();
        }

        #endregion
    }
}
