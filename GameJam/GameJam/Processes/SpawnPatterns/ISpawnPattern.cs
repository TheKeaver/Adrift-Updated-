using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Processes.SpawnPatterns
{
    public interface ISpawnPattern
    {
        float GetMaxSpawnTimer();
        int GetNumberOfValidCenters();
        // Optional. If not stored in the SpawnPattern, however, we are relying on this logic being stored in the SpawnPatternManager
        float GetMaximumValidRadius();
    }
}
