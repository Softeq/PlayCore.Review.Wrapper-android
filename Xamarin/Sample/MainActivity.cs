using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Com.Softeq.Playcorewrapper;
using Xamarin.Google.Android.Play.Core.Review;
using Xamarin.Google.Android.Play.Core.Tasks;

namespace Sample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private RequestReviewService _requestReviewService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            _requestReviewService = new RequestReviewService();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            //_requestReviewService.RequestReview(this, new ReviewListener());

            var rs = ReviewManagerFactory.Create(this);
            var task = rs.RequestReviewFlow();
            task.AddOnCompleteListener(new CustomOnCompleteListener());
            task.AddOnFailureListener(new CustomOnFailureListener());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private class CustomOnCompleteListener : Java.Lang.Object, IOnCompleteListener
        {
            public void OnComplete(Task p0)
            {
                Console.WriteLine("User has completed native App review");
            }
        }

        private class CustomOnFailureListener : Java.Lang.Object, IOnFailureListener
        {
            public void OnFailure(Java.Lang.Exception p0)
            {
                Console.WriteLine("Native App review has failed");
            }
        }

        //private class ReviewListener : Java.Lang.Object, IReviewListener
        //{
        //    public void OnError()
        //    {
        //        Console.WriteLine("Native App review has failed");
        //    }

        //    public void OnSuccess()
        //    {
        //        Console.WriteLine("User has completed native App review");
        //    }
        //}
    }
}
