#if (DEBUG)
// comment these defines to selectively simulate some components
//#define REAL_SHELLY3EM
//#define REAL_MYSTROMSWITCH
#else
  // do not change this section
#define REAL_SHELLY3EM
#define REAL_MYSTROMSWITCH
#endif

namespace EnergyMonitor {
  class Simulation {

    public static bool Simulate_PowerMeter() {
#if (REAL_SHELLY3EM)
      return false;
#else
      return true;
#endif
    }

    public static bool Simulate_PowerSwitch() {
#if (REAL_MYSTROMSWITCH)
      return false;
#else
      return true;
#endif
    }
  }
}