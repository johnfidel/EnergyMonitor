#if (DEBUG)
// comment these defines to selectively simulate some components
//#define REAL_SHELLY3EM
#else
  // do not change this section
#define REAL_SHELLY3EM
#endif

namespace EnergyMonitor {   
  class Simulation {

    public static bool Simulate_Shelly3EM() {
#if (REAL_SHELLY3EM)
      return false;
#else
      return true;
#endif
    }
  }
}