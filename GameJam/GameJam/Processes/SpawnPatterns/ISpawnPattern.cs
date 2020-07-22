using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Processes.SpawnPatterns
{
    public interface ISpawnPattern
    {
        /*
        * "Animation chasing enemy spawn duration": Is complete spawn time, for Chasing enemy in seconds
        * Warp Equation: (Warp Animtation Phase 1 + Warp Animation Phase 2) * Warp Time Scale, for Shooting and Laser enemy in seconds
        */
        float GetMaxSpawnTimer();
        int GetNumberOfValidCenters();
        // Optional. If not stored in the SpawnPattern, however, we are relying on this logic being stored in the SpawnPatternManager
        float GetMinimumValidRadius();
    }
}
