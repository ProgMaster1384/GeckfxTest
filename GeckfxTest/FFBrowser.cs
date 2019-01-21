namespace GeckfxTest
{

	public class FFBrowser : System.Windows.Forms.ApplicationContext
	{

		public System.Threading.AutoResetEvent _ResultEvent;

		public int _NavigationCounter;
		public bool _IsDone = false;

		private Gecko.GeckoWebBrowser _FFBrowser;
		private System.Threading.Thread _Thread;
		private System.Windows.Forms.Form _Form;

		public FFBrowser( bool visible, System.Threading.AutoResetEvent resultEvent )
		{
			this._ResultEvent = resultEvent;

			this._Thread = new System.Threading.Thread( new System.Threading.ThreadStart(
			    delegate
			    {
				    this._NavigationCounter = 0;

				    Gecko.Xpcom.EnableProfileMonitoring = false;
				    Gecko.Xpcom.Initialize( System.Web.Hosting.HostingEnvironment.MapPath( "~/Firefox" ) );


				    this._FFBrowser = new Gecko.GeckoWebBrowser();

				    this._FFBrowser.Navigating += this.FFBrowser_Navigating;
				    this._FFBrowser.DocumentCompleted += this.FFBrowser_DocumentCompleted;

				    if ( visible )
				    {
					    this._Form = new System.Windows.Forms.Form();
					    this._Form.WindowState = System.Windows.Forms.FormWindowState.Normal;
					    this._Form.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
					    this._Form.Height = 850;
					    this._Form.Width = 1100;
					    this._Form.Top = 20;
					    this._Form.Left = 15;
					    this._FFBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
					    this._Form.Controls.Add( this._FFBrowser );
					    this._Form.Visible = true;
				    }


				    this._FFBrowser.Navigate( "https://duckduckgo.com" );


				    System.Windows.Forms.Application.Run( this );


			    } ) );

			// set thread to STA state before starting
			this._Thread.SetApartmentState( System.Threading.ApartmentState.STA );
			this._Thread.Start();
		}

		private void FFBrowser_Navigating( object sender, Gecko.Events.GeckoNavigatingEventArgs e )
		{
			// Navigation count increases by one
			this._NavigationCounter++;

		}
		private void FFBrowser_DocumentCompleted( object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e )
		{
			Gecko.GeckoDocument doc = this._FFBrowser.Document;

			System.Windows.Forms.Application.DoEvents();
			System.Threading.Thread.Sleep( 500 );

			this._IsDone = true;

			this._ResultEvent.Set();


		}



		protected override void Dispose( bool disposing )
		{
			if ( this._Thread != null )
			{
				this._Thread.Abort();
				this._Thread = null;
				return;
			}


			System.Runtime.InteropServices.Marshal.Release( _FFBrowser.Handle );
			this._FFBrowser.Dispose();

			if ( this._Form != null )
				this._Form.Dispose();

			base.Dispose( disposing );
		}


	}

}
