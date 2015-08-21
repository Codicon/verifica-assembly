﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace VerificarAssembly
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtDllDebug.Text = "";
            txtDllRelease.Text = "";
            string diretorio = txtDiretorio.Text;

            if (!Directory.Exists(diretorio))
            {
                lblInfo.Text = "Diretório Inválido";
                return;
            }

            List<string> listaArquivosDeb = new List<string>();
            List<string> listaArquivosRel = new List<string>();

            string[] filePaths = Directory.GetFiles(diretorio, "*.dll");
            string nomeDll;

            foreach (string arquivos in filePaths)
            {
                var a = Assembly.LoadFile(arquivos);

                object[] attribs = a.GetCustomAttributes(typeof(DebuggableAttribute), false);

                if (attribs.Length <= 0)
                    continue;

                DebuggableAttribute debuggableAttribute = attribs[0] as DebuggableAttribute;

                if (debuggableAttribute == null)
                    continue;

                nomeDll = a.FullName.Split(',')[0] + " " + a.FullName.Split(',')[1];

                if (debuggableAttribute.IsJITOptimizerDisabled)//TRUE == DEBUG                            
                    listaArquivosDeb.Add(nomeDll);
                else
                    listaArquivosRel.Add(nomeDll);
            }
            foreach (var item in listaArquivosDeb)
                txtDllDebug.AppendText(item + Environment.NewLine);
            foreach (var item in listaArquivosRel)
                txtDllRelease.AppendText(item + Environment.NewLine);


        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtDiretorio.Text = folderBrowserDialog1.SelectedPath;
                button1_Click(null, null);
            }
        }

        private void txtDiretorio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(null, null);
        }


    }
}
