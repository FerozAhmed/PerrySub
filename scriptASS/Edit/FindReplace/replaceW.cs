using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace scriptASS
{
    public partial class replaceW : Form
    {
        mainW mw;
        ArrayList found = new ArrayList();
        int PosicionActual = -1;

        public replaceW(mainW m)
        {
            InitializeComponent();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            mw = m;

            comboReplace.TextChanged += new EventHandler(comboReplace_TextChanged);
        }

        void comboReplace_TextChanged(object sender, EventArgs e)
        {
            found.Clear();
            PosicionActual = -1;
            button2.Enabled = button3.Enabled = button4.Enabled = false;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void replaceW_Load(object sender, EventArgs e)
        {
            try
            {
                string[] historial = mw.getFromConfigFileA("replaceW_ReplaceHistorial");
                Array.Reverse(historial);
                for (int i = 0; i < historial.Length; i++)
                {
                    comboReplace.Items.Add(historial[i].Replace('※', ',').Replace('卍', '"'));
                    comboReplaceTo.Items.Add(historial[i].Replace('※', ',').Replace('卍', '"'));
                }
            }
            catch { }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            found.Clear();

            for (int i = 0; i < mw.script.LineCount; i++)
            {
                bool ismatch = false;
                lineaASS actual = (lineaASS)mw.script.GetLines()[i];

                if (buscarTexto.Checked)
                {
                    if (regExp.Checked)
                    {
                        try
                        {
                            Regex r = new Regex(comboReplace.Text);
                            ismatch = r.IsMatch(actual.texto);
                        }
                        catch
                        {
                            mw.errorMsg("Fallo al compilar la expresión regular");
                            return;
                        }
                    }
                    else
                    {
                        ismatch = (caseInSensitive.Checked) ?
                            (actual.texto.ToLower().Contains(comboReplace.Text.ToLower())) :
                            (actual.texto.Contains(comboReplace.Text));
                    }
                }
                else if (buscarEstilos.Checked)
                {
                    if (regExp.Checked)
                    {
                        try
                        {
                            Regex r = new Regex(comboReplace.Text);
                            ismatch = r.IsMatch(actual.estilo);
                        }
                        catch
                        {
                            mw.errorMsg("Fallo al compilar la expresión regular");
                            return;
                        }
                    }
                    else
                    {
                        ismatch = (caseInSensitive.Checked) ?
                            (actual.estilo.ToLower().Contains(comboReplace.Text.ToLower())) :
                            (actual.estilo.Contains(comboReplace.Text));
                    }
                }
                else if (buscarPersonajes.Checked)
                {
                    if (regExp.Checked)
                    {
                        try
                        {
                            Regex r = new Regex(comboReplace.Text);
                            ismatch = r.IsMatch(actual.personaje);
                        }
                        catch
                        {
                            mw.errorMsg("Fallo al compilar la expresión regular");
                            return;
                        }
                    }
                    else
                    {
                        ismatch = (caseInSensitive.Checked) ?
                            (actual.personaje.ToLower().Contains(comboReplace.Text.ToLower())) :
                            (actual.personaje.Contains(comboReplace.Text));
                    }
                }

                if (ismatch) found.Add(i);
            }

            if (rangoSeleccionadas.Checked)
                found = mw.TrimToSelected(found);

            //mw.doFind(found, rangoSeleccionadas.Checked);
            button2.Enabled = (found.Count > 0);
            button3.Enabled = (found.Count > 0);
            button4.Enabled = (found.Count > 0);
            
            // ※ - comas
            // 卍 - comillas
            string parsed_text1 = comboReplace.Text.Replace(',', '※').Replace('"', '卍');
            string parsed_text2 = comboReplaceTo.Text.Replace(',', '※').Replace('"', '卍');
            mw.updateConcatenateConfigFile("replaceW_ReplaceHistorial", parsed_text1);
            mw.updateConcatenateConfigFile("replaceW_ReplaceHistorial", parsed_text2);

            if (found.Count == 0)
            {
                mw.errorMsg("No se han encontrado coincidencias.");
                return;
            }

            // la sustitucion ._.
            int idx = (int)found[0];
            mw.clearSelectedRows();
            mw.selectRow(idx);
            mw.moveViewRows(idx);
            PosicionActual = 0;
        }



        private void Replace(int pos)
        {
            int idx = (int)found[pos];
            lineaASS actual = (lineaASS)mw.script.GetLines()[idx];

            if (buscarTexto.Checked)
            {
                if (regExp.Checked)
                {
                    Regex r = new Regex(comboReplace.Text);
                    MatchCollection mc = r.Matches(actual.texto);
                    foreach (Match m in mc)
                    {
                        actual.texto = actual.texto.Replace(m.Value, comboReplaceTo.Text);
                    }
                }
                else
                {
                    actual.texto = actual.texto.Replace(comboReplace.Text, comboReplaceTo.Text);
                }
            }
            else if (buscarEstilos.Checked)
            {
                if (regExp.Checked)
                {
                    Regex r = new Regex(comboReplace.Text);
                    MatchCollection mc = r.Matches(actual.estilo);
                    foreach (Match m in mc)
                    {
                        actual.estilo = actual.estilo.Replace(m.Value, comboReplaceTo.Text);
                    }
                }
                else
                {
                    actual.estilo = actual.estilo.Replace(comboReplace.Text, comboReplaceTo.Text);
                }
            }
            else if (buscarPersonajes.Checked)
            {
                if (regExp.Checked)
                {
                    Regex r = new Regex(comboReplace.Text);
                    MatchCollection mc = r.Matches(actual.personaje);
                    foreach (Match m in mc)
                    {
                        actual.personaje = actual.personaje.Replace(m.Value, comboReplaceTo.Text);
                    }

                }
                else
                {
                    actual.personaje = actual.personaje.Replace(comboReplace.Text, comboReplaceTo.Text);
                }
            }
            mw.updateGridWithArrayList(mw.al);
        }

        private void OneForward()
        {
            PosicionActual++;
            if (PosicionActual == found.Count) PosicionActual = 0;

            int idx = (int)found[PosicionActual];
            mw.clearSelectedRows();
            mw.selectRow(idx);
            mw.moveViewRows(idx);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OneForward();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Replace(PosicionActual);
            OneForward();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < found.Count; i++)
                Replace(i);
        }
    }
}