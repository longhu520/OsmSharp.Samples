using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using OsmSharp.Android.UI;

namespace Android.Demo
{
    [Activity(Label = "Android.Demo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // initialize OsmSharp native hooks.
            Native.Initialize();

            // enable the logging.
            OsmSharp.Logging.Log.Enable();
            OsmSharp.Logging.Log.RegisterListener(new OsmSharp.Android.UI.Log.LogTraceListener());

            base.OnCreate(bundle);

            var layout = new LinearLayout(this);
            layout.Orientation = Orientation.Vertical;

            var mBTilesButton = new Button(this);
            mBTilesButton.Text = "Offline Tiles Demo";
            mBTilesButton.Click += (sender, e) =>
            {
                StartActivity(typeof(MBTilesActivity));
            };
            layout.AddView(mBTilesButton);

            var tilesActivityButton = new Button(this);
            tilesActivityButton.Text = "Online Tiles Demo";
            tilesActivityButton.Click += (sender, e) =>
            {
                StartActivity(typeof(TilesActivity));
            };
            layout.AddView(tilesActivityButton);

            var vectorActivityButton = new Button(this);
            vectorActivityButton.Text = "Offline Vector Map Demo";
            vectorActivityButton.Click += (sender, e) =>
            {
                StartActivity(typeof(VectorActivity));
            };
            layout.AddView(vectorActivityButton);



            SetContentView(layout);
        }
    }
}

