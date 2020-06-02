using System;

namespace consoleApp
{
    public enum Lifeline
    {
        HealthMedical,
        FoodWaterShelter,
        SafetySecurity,
        HazardousMaterial,
        Transportation,
        Communication,
        Energy
    }

    static class Extensions
    {
        public static Lifeline GetRandomLifeline()
        {
            Array values = Enum.GetValues(typeof(Lifeline));
            Random random = new Random();
            Lifeline randomLifeline = (Lifeline)values.GetValue(random.Next(values.Length));
            return randomLifeline;
        }
    }

    
}
