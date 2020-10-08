namespace AforgeCameraSettings
{
    using AForge.Video;
    using AForge.Video.DirectShow;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;


    /// <summary>
    /// Defines the <see cref="Viewer" />.
    /// </summary>
    public partial class Viewer : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Viewer"/> class.
        /// </summary>
        public Viewer()
        {
            InitializeComponent();

            LoadCameraList();
        }

        // List of filterInfo for each attached camera
        // No good pluging a camera in after the app has started
        /// <summary>
        /// Defines the filterInfo.
        /// </summary>
        List<FilterInfo> filterInfo = new List<FilterInfo>();

        // The indiex in above list for he selected camera
        /// <summary>
        /// Defines the cameraIndex.
        /// </summary>
        int cameraIndex = -1;// Not set

        /// <summary>
        /// Defines the videoSource.
        /// </summary>
        VideoCaptureDevice videoSource;

        /// <summary>
        /// The toolStripComboBoxCameraSelection_TextChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private async void toolStripComboBoxCameraSelection_TextChanged(object sender, EventArgs e)
        {
            cameraIndex = toolStripComboBox1CameraList.SelectedIndex;
            await StartCamera(cameraIndex);
        }

        /// <summary>
        /// The LoadCameraList.
        /// </summary>
        private void LoadCameraList()
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            for (int i = 0; i < videoDevices.Count; i++)
            {
                if (!(videoDevices[i].Name == "Logi Capture"))
                {
                    filterInfo.Add(videoDevices[i]);
                    toolStripComboBox1CameraList.Items.Add(videoDevices[i].Name);
                }
            }
        }

        /// <summary>
        /// The StartCamera.
        /// </summary>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task StartCamera(int index)
        {
            StopCamera();  // Try it  to be sure to be sure

            // create video source
            videoSource = new VideoCaptureDevice(filterInfo[cameraIndex].MonikerString);

            // set NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            // start the video source
            videoSource.Start();
            Task<int> i = waitabit();
        }

        /// <summary>
        /// The waitabit.
        /// </summary>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        private async Task<int> waitabit()
        {
            await Task.Delay(2000);
            return 1;
        }

        /// <summary>
        /// The StopCamera.
        /// </summary>
        private void StopCamera()
        {
            if (!(videoSource == null))
            {
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                }
            }
        }

        /// <summary>
        /// The Cameraparameters.
        /// </summary>
        private void Cameraparameters()
        {
            if (!(videoSource == null))
            {
                if (videoSource.IsRunning)
                {
                    videoSource.DisplayPropertyPage(IntPtr.Zero);
                    //var x = videoSource.VideoCapabilities;
                    //var y = videoSource.VideoResolution;

                }
            }
        }

        /// <summary>
        /// The video_NewFrame.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="eventArgs">The eventArgs<see cref="NewFrameEventArgs"/>.</param>
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bm = AForge.Imaging.Image.Clone(eventArgs.Frame);
            pictureBox1.Image = bm;
        }

        /// <summary>
        /// The toolStripMenuItem1Camerasettings_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void toolStripMenuItem1Camerasettings_Click(object sender, EventArgs e)
        {
            Cameraparameters();
        }

        // End Camera stuff
        /// <summary>
        /// The exitToolStripMenuItem_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            StopCamera();
        }

        /// <summary>
        /// The toolStripMenuItemCameraSettings_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void toolStripMenuItemCameraSettings_Click(object sender, EventArgs e)
        {
            Cameraparameters();
        }

        /// <summary>
        /// The toolStripComboBox1CameraList_SelectedIndexChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private async void toolStripComboBox1CameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cameraIndex = toolStripComboBox1CameraList.SelectedIndex;
            await StartCamera(cameraIndex);
        }

        /// <summary>
        /// The Viewer_FormClosing.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="FormClosingEventArgs"/>.</param>
        private void Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();
        }

        /// <summary>
        /// The aboutToolStripMenuItem_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form about = new About();

            about.ShowDialog();
        }
    }
}
