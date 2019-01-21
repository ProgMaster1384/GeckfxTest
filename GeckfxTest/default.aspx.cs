using System;

namespace GeckfxTest
{
	public partial class _default : System.Web.UI.Page
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			for ( int i = 0; i < 10; i++ )
			{
				try
				{


					System.Threading.AutoResetEvent resultevent = new System.Threading.AutoResetEvent( false );

					bool visible = false; // Display the WebBrowser form

					FFBrowser browser = new FFBrowser( visible, resultevent );

					// Wait for the third thread getting result and setting result event
					while ( browser._IsDone == false )
					{
						System.Threading.EventWaitHandle.WaitAll( new System.Threading.AutoResetEvent[] { resultevent } );
					}


					if ( visible )
						browser.Dispose();


					System.Threading.Thread.Sleep( 5000 );
				}
				catch ( Exception ex )
				{
					throw ex;
				}
			}





		}
	}
}