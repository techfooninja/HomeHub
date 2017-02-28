namespace HomeHub.Shared
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Temperature
    {
        [DataMember]
        public TemperatureScale Scale;

        [DataMember]
        public float Degrees;

        public Temperature()
        {
            Scale = TemperatureScale.Celsius;
            Degrees = 0;
        }

        public Temperature(TemperatureScale scale, float degrees)
        {
            Scale = scale;
            Degrees = degrees;
        }

        public Temperature(Temperature original)
        {
            Scale = original.Scale;
            Degrees = original.Degrees;
        }

        public Temperature ConvertToScale(TemperatureScale scale)
        {
            Temperature newTemp = new Temperature(this);

            if (Scale == scale)
            {
                // Nothing to do, return clone
                return newTemp;
            }

            switch (Scale)
            {
                case TemperatureScale.Celsius:
                    switch (scale)
                    {
                        case TemperatureScale.Fahrenheit:
                            newTemp.Degrees = ConvertCelciusToFahrenheit(newTemp.Degrees);
                            break;
                        case TemperatureScale.Kelvin:
                            newTemp.Degrees = ConvertCelciusToKelvin(newTemp.Degrees);
                            break;
                    }
                    break;
                case TemperatureScale.Fahrenheit:
                    newTemp.Degrees = ConvertFahrenheitToCelcius(newTemp.Degrees);
                    switch (scale)
                    {
                        case TemperatureScale.Kelvin:
                            newTemp.Degrees = ConvertCelciusToKelvin(newTemp.Degrees);
                            break;
                    }
                    break;
                case TemperatureScale.Kelvin:
                    newTemp.Degrees = ConvertKelvinToCelcius(newTemp.Degrees);
                    switch (scale)
                    {
                        case TemperatureScale.Fahrenheit:
                            newTemp.Degrees = ConvertCelciusToFahrenheit(newTemp.Degrees);
                            break;
                    }
                    break;
            }
            newTemp.Scale = scale;
            return newTemp;
        }

        #region Operators

        public static bool operator <(Temperature temp1, Temperature temp2)
        {
            Temperature temp2a = temp2.ConvertToScale(temp1.Scale);
            if (temp1.Degrees < temp2a.Degrees)
            {
                return true;
            }
            return false;
        }

        public static bool operator <=(Temperature temp1, Temperature temp2)
        {
            return temp1 < temp2 || temp1 == temp2;
        }

        public static bool operator >(Temperature temp1, Temperature temp2)
        {
            Temperature temp2a = temp2.ConvertToScale(temp1.Scale);
            if (temp1.Degrees > temp2a.Degrees)
            {
                return true;
            }
            return false;
        }

        public static bool operator >=(Temperature temp1, Temperature temp2)
        {
            return temp1 > temp2 || temp1 == temp2;
        }

        public static bool operator ==(Temperature temp1, Temperature temp2)
        {
            if (((object)temp1) == null)
            {
                if (((object)temp2) == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return temp1.Equals(temp2);
        }

        public static bool operator !=(Temperature temp1, Temperature temp2)
        {
            if (((object)temp1) == null)
            {
                if (((object)temp2) == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return !temp1.Equals(temp2);
        }

        public static Temperature operator +(Temperature temp1, Temperature temp2)
        {
            Temperature temp2a = temp2.ConvertToScale(temp1.Scale);
            return new Temperature(temp1.Scale, temp1.Degrees + temp2a.Degrees);
        }

        public static Temperature operator -(Temperature temp1, Temperature temp2)
        {
            Temperature temp2a = temp2.ConvertToScale(temp1.Scale);
            return new Temperature(temp1.Scale, temp1.Degrees - temp2a.Degrees);
        }

        public static Temperature operator *(Temperature temp1, float multiplier)
        {
            return new Temperature(temp1.Scale, temp1.Degrees * multiplier);
        }

        public static Temperature operator /(Temperature temp1, float divisor)
        {
            return new Temperature(temp1.Scale, temp1.Degrees / divisor);
        }

        public static Temperature Average(IEnumerable<Temperature> temps)
        {
            int tempCount = 0;
            Temperature average = null;

            // TODO: Extension method for average?
            foreach (Temperature temp in temps)
            {
                if (average == null)
                {
                    // First temperature
                    average = temp;
                }
                else
                {
                    average += temp;
                }
                tempCount++;
            }

            average = average / tempCount;
            return average;
        }

        #endregion Operators

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Temperature)
            {
                Temperature temp2 = ((Temperature)obj).ConvertToScale(Scale);
                if (Degrees == temp2.Degrees)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 486187739 + Degrees.GetHashCode();
                hash = hash * 486187739 + Scale.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            string scaleStr = "";

            switch (Scale)
            {
                case TemperatureScale.Celsius:
                    scaleStr = "°C";
                    break;
                case TemperatureScale.Fahrenheit:
                    scaleStr = "°F";
                    break;
                case TemperatureScale.Kelvin:
                    scaleStr = "K";
                    break;
            }

            return string.Format("{0:0.0}{1}", Degrees, scaleStr);
        }

        #region Private Helpers

        private float ConvertCelciusToFahrenheit(float degrees)
        {
            return ((degrees * 9) / 5) + 32;
        }

        private float ConvertFahrenheitToCelcius(float degrees)
        {
            return ((degrees - 32) / 9) * 5;
        }

        private float ConvertCelciusToKelvin(float degrees)
        {
            return degrees + 273.15f;
        }

        private float ConvertKelvinToCelcius(float degrees)
        {
            return degrees - 273.15f;
        }

        #endregion Private Helpers
    }
}
