using System;
using System.Windows.Forms;

using MitsubishiModbus.Classes;

namespace MitsubishiModbus
{
    public partial class ViewFrm : Form
    {
        private Modbus_FC1_FC5 _Modbus_FC1_FC5;

        public ViewFrm()
        {
            InitializeComponent();
        }

        private void ViewFrm_Load(object sender, EventArgs e)
        {
            _Modbus_FC1_FC5 = new Modbus_FC1_FC5(16, 16, 0x2000, 0);
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                _Modbus_FC1_FC5.SetOutput(UInt16.Parse(txtOutReg.Text), txtOutVal.Text == "1");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                txtInVal.Text = _Modbus_FC1_FC5.GetInput(UInt16.Parse(txtInReg.Text)).ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                _Modbus_FC1_FC5.Connect("192.168.3.250", cbInitOutputs.Checked);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _Modbus_FC1_FC5.Disconnect();
        }
    }
}
