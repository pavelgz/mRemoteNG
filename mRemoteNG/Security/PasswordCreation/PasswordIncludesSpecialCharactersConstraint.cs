﻿using System;
using System.Collections.Generic;
using System.Security;
using System.Text.RegularExpressions;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.Security.PasswordCreation
{
    public class PasswordIncludesSpecialCharactersConstraint : IPasswordConstraint
    {
        private readonly int _minimumCount;

        public IEnumerable<char> SpecialCharacters { get; } = new[] {'!', '@', '#', '$', '%', '^', '&', '*'};

        public string ConstraintHint { get; }

        public PasswordIncludesSpecialCharactersConstraint(int minimumCount = 1)
        {
            if (minimumCount < 0)
                throw new ArgumentException($"{nameof(minimumCount)} must be a positive value");

            _minimumCount = minimumCount;
        }

        public PasswordIncludesSpecialCharactersConstraint(IEnumerable<char> specialCharacters, int minimumCount = 1)
            : this(minimumCount)
        {
            if (specialCharacters == null)
                throw new ArgumentNullException(nameof(specialCharacters));

            SpecialCharacters = specialCharacters;
            ConstraintHint = string.Format(Language.PasswordConstainsSpecialCharactersConstraintHint, _minimumCount,
                                           string.Concat(SpecialCharacters));
        }

        public bool Validate(SecureString password)
        {
            Regex regex = new($"[{string.Concat(SpecialCharacters)}]");
            return regex.Matches(password.ConvertToUnsecureString()).Count >= _minimumCount;
        }
    }
}