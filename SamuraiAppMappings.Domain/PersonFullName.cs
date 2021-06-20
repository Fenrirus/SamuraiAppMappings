using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace SamuraiAppMappings.Domain
{
    public class PersonFullName
    {
        public PersonFullName(string givenName, string surName)
        {
            GivenName = givenName;
            SurName = surName;
        }

        private PersonFullName()
        {
        }

        public string FullName => $"{GivenName} {SurName}";
        public string FullNameReverse => $"{SurName} {GivenName}";
        public string GivenName { get; private set; }
        public string SurName { get; private set; }

        public static bool operator !=(PersonFullName left, PersonFullName right)
        {
            return !(left == right);
        }

        public static bool operator ==(PersonFullName left, PersonFullName right)
        {
            return EqualityComparer<PersonFullName>.Default.Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            return obj is PersonFullName name &&
                   GivenName == name.GivenName &&
                   SurName == name.SurName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GivenName, SurName);
        }
    }
}