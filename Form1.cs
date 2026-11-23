using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace MiniBrauzer
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private void Form1_Load(object sender, EventArgs e)
        {
            listSecilmisler.Items.AddRange(new[] { "google.com", "wikipedia.org", "youtube.com", "azertag.az" });
            YeniTab("https://www.google.com");
        }

        private async void YeniTab(string url)
        {
            var tab = new TabPage("Tab");
            var web = new WebView2 { Dock = DockStyle.Fill };
            tab.Controls.Add(web);
            tabControl1.TabPages.Add(tab);
            tabControl1.SelectedTab = tab;

            await web.EnsureCoreWebView2Async();
            web.NavigationCompleted += (s, e) => {
                tab.Text = web.CoreWebView2.DocumentTitle;
                txtUrl.Text = web.Source.ToString();
                if (!listTarixce.Items.Contains(txtUrl.Text)) listTarixce.Items.Add(txtUrl.Text);
            };
            Get(url);
        }

        private WebView2 AktivWeb() => tabControl1.SelectedTab?.Controls[0] as WebView2;

        private void Get(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            if (!url.StartsWith("http")) url = "https://" + url;
            AktivWeb()?.CoreWebView2?.Navigate(url);
        }

        private void btnGet_Click(object sender, EventArgs e) => Get(txtUrl.Text);
        private void btnGeri_Click(object sender, EventArgs e) => AktivWeb()?.GoBack();
        private void btnIreli_Click(object sender, EventArgs e) => AktivWeb()?.GoForward();
        private void btnYenile_Click(object sender, EventArgs e) => AktivWeb()?.Reload();
        private void btnAnaSehife_Click(object sender, EventArgs e) => Get("google.com");
        private void btnYeniTab_Click(object sender, EventArgs e) => YeniTab("google.com");

        private void btnElaveEt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUrl.Text)) listSecilmisler.Items.Add(txtUrl.Text);
        }

        private void listSecilmisler_SelectedIndexChanged(object sender, EventArgs e) => Get(listSecilmisler.SelectedItem?.ToString());
        private void listTarixce_SelectedIndexChanged(object sender, EventArgs e) => Get(listTarixce.SelectedItem?.ToString());
    }
}
