#ifndef GLOBALSNOW_DEFERRED_OPTIONS
#define GLOBALSNOW_DEFERRED_OPTIONS

// ************* Global shader feature options ********************

// Uncomment to support coverage mask on grass and trees in deferred rendering path
//#define GLOBALSNOW_MASK

// Comment out to disable zenithal depth
#define USE_ZENITHAL_DEPTH

// Uncomment and adjust distance to exclude First Person weapons from snow (if required)
//#define EXCLUDE_NEAR_SNOW
#define NEAR_DISTANCE_SNOW 0.00002

    
#endif