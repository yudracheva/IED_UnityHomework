using System;

namespace UserSettings
{
    public class GeneralUserSettings : ICloneable
    {
        public float MusicVolume { get; set; }
    
        public float ActionsVolume { get; set; }
        
        public object Clone()
        {
            return new GeneralUserSettings() { MusicVolume = this.MusicVolume, ActionsVolume = this.ActionsVolume};
        }
    }   
}
