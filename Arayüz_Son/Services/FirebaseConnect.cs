using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FireSharp.Config;
using FireSharp.Interfaces;


namespace Arayüz_Son.Services
{
    internal class FirebaseConnect
    {
        private static FirebaseConnect _instance;
        private IFirebaseClient client;

        public static FirebaseConnect Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FirebaseConnect();
                }
                return _instance;
            }
        }

        private FirebaseConnect()
        {
            IFirebaseConfig fc = new FirebaseConfig()
            {
                AuthSecret = "iBmOzWiPc6jJSAcAoEbmHEujviHYKPEMjy3vI3sf",
                BasePath = "https://duscartotonom-1419d-default-rtdb.europe-west1.firebasedatabase.app/"
            };
            try
            {
                client = new FireSharp.FirebaseClient(fc);
               // MessageBox.Show("Baglandı!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public IFirebaseClient GetClient()
        {
            return client;
        }
    }
}
