﻿using Sacados.Containers;

namespace Sacados.Examples.FixedContainer {

    public partial class FixedContainer : GUIContainer {

        /**
          * 
          * Common code of the Fixed Container
          * 
          **/

        private void Update() {

            // Here we just call two update methods (for the client and the server)
            // Because we can't have two Update() method in one class

#if IS_CLIENT
            ClientUpdate();
#endif

#if IS_SERVER
            ServerUpdate();
#endif

        }

    }


}
