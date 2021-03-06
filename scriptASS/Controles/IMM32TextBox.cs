using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace scriptASS
{
    class IMM32TextBox : TextBox
    {
        private TextBox furigana;
        private TextBox roomaji;

        public TextBox FuriganaTextBox
        {
            get { return furigana; }
            set { furigana = value; }
        }

        public TextBox RoomajiTextBox
        {
            get { return roomaji; }
            set { roomaji = value; }
        }
        
                private string KatakanaToRoomaji(string kata)
                {
                    string[] rom = {"a",    "i",    "u",    "e",    "o",
                                    "ga",   "gi",   "gu",   "ge",   "go",
                                    "ka",   "ki",   "ku",   "ke",   "ko",
                                    "da",   "di",   "du/zu","de",   "do",
                                    "ta",   "chi",  "tsu",  "te",   "to",
                                    "sa",   "shi",  "su",   "se",   "so",
                                    "ba",   "bi",   "bu",   "be",   "bo",
                                    "pa",   "pi",   "pu",   "pe",   "po",
                                    "fa",   "fi",           "fe",   "fo",
                                    "ha/wa","hi",   "fu",   "he/e", "ho",
                                    "ma",   "mi",   "mu",   "me",   "mo",
                                    "na",   "ni",   "nu",   "ne",   "no",
                                    "ra",   "ri",   "ru",   "re",   "ro",
                                    "ya",           "yu",           "yo",
                                    "wa",                           "wo/o",
                                    "n"};
                    string[] kat = {"ｱ",    "ｲ",    "ｳ",    "ｴ",    "ｵ",
                                    "ｶﾞ",   "ｷﾞ",   "ｸﾞ",   "ｹﾞ",   "ｺﾞ",
                                    "ｶ",    "ｷ",    "ｸ",    "ｹ",    "ｺ",
                                    "ﾀﾞ",   "ﾁﾞ",   "ﾂﾞ",   "ﾃﾞ",   "ﾄﾞ",
                                    "ﾀ",    "ﾁ",    "ﾂ",    "ﾃ",    "ﾄ",
                                    "ｻ",    "ｼ",    "ｽ",    "ｾ",    "ｿ",
                                    "ﾊﾞ",   "ﾋﾞ",   "ﾌﾞ",   "ﾍﾞ",   "ﾎﾞ",                                    
                                    "ﾊﾟ",   "ﾋﾟ",   "ﾌﾟ",   "ﾍﾟ",   "ﾎﾟ",
                                    "ﾌｧ",   "ﾌｨ",           "ﾌｪ",   "ﾌｫ",
                                    "ﾊ",    "ﾋ",    "ﾌ",    "ﾍ",    "ﾎ",
                                    "ﾏ",    "ﾐ",    "ﾑ",    "ﾒ",    "ﾓ",
                                    "ﾅ",    "ﾆ",    "ﾇ",    "ﾈ",    "ﾉ",
                                    "ﾗ",    "ﾘ",    "ﾙ",    "ﾚ",    "ﾛ",
                                    "ﾔ",            "ﾕ",            "ﾖ",
                                    "ﾜ",                            "ｦ",
                                    "ﾝ"};


                    for (int i = 0; i < kat.Length; i++)
                        kata = kata.Replace(kat[i], "["+rom[i]+"] ");

                    return kata;

                }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            int hIMC = 0;
            if (m.Msg == IMM32Wrapper.WM_IME_COMPOSITION && furigana!=null)
            {                
                IntPtr hwndptr = this.Handle;
                int hwnd = hwndptr.ToInt32();
                hIMC = IMM32Wrapper.ImmGetContext(hwnd);

                int len = IMM32Wrapper.ImmGetCompositionStringW(hIMC, IMM32Wrapper.GCS_RESULTREADSTR, null, 0);
                if (len > 0)
                {
                    byte[] bytearray = new byte[len*2];
                    IMM32Wrapper.ImmGetCompositionStringW(hIMC, IMM32Wrapper.GCS_RESULTREADSTR, bytearray, len);
                    FuriganaTextBox.Text = Encoding.Unicode.GetString(bytearray);
                    RoomajiTextBox.Text = KatakanaToRoomaji(FuriganaTextBox.Text);
                }

                IMM32Wrapper.ImmReleaseContext(hwnd, hIMC);

            }   
                /*
            else if (m.Msg == IMM32Wrapper.WM_CHAR)
            {                
                IntPtr hwndptr = this.Handle;
                int hwnd = hwndptr.ToInt32();

                hIMC = IMM32Wrapper.ImmGetContext(hwnd);

                if (IMM32Wrapper.ImmGetOpenStatus(hIMC) == 0)
                {
                    if (m.WParam.ToInt32() >= 32)
                    {
                        MessageBox.Show("MEMB");
                    }
                }

                IMM32Wrapper.ImmReleaseContext(hwnd, hIMC);
            }
                 */ 
            base.WndProc(ref m);
        }

    }
}
