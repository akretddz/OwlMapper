using Account.Core.Identity;

using static Account.Core.Identity.Consts.PasswordHasher;

namespace Account.Tests.Unit.Core.Identity
{
    [TestFixture]
    internal class PasswordHasherTests
    {
        private PasswordHasher _target = null!;

        [SetUp]
        public void Before_Each_Test() => _target = new PasswordHasher();

        private class HashPassword : PasswordHasherTests
        {
            [TestCase("password")]
            [TestCase("P@ssw0rd!")]
            [TestCase("a")]
            [TestCase("very long password that is definitely longer than most passwords used in practice 1234567890!@#$%^&*()")]
            [TestCase("zażółć gęślą jaźń")]
            public void HashPassword_ReturnsBase64String(string password)
            {
                var hash = _target.HashPassword(password);

                Assert.That(() => Convert.FromBase64String(hash), Throws.Nothing);
            }

            [TestCase("password")]
            [TestCase("P@ssw0rd!")]
            public void HashPassword_ReturnsDifferentHashesForSamePassword(string password)
            {
                var hash1 = _target.HashPassword(password);
                var hash2 = _target.HashPassword(password);

                Assert.That(hash1, Is.Not.EqualTo(hash2));
            }

            [TestCase("password")]
            [TestCase("P@ssw0rd!")]
            public void HashPassword_ReturnedHash_HasCorrectLength(string password)
            {
                const int expectedLength = SaltSize + HashSize;

                var hash = _target.HashPassword(password);

                var bytes = Convert.FromBase64String(hash);
                Assert.That(bytes.Length, Is.EqualTo(expectedLength));
            }
        }

        private class VerifyPasswordTests : PasswordHasherTests
        {

            private static readonly string[] ValidPasswords =
            [
                "password",
                "P@ssw0rd!",
                "a",
                "very long password that is definitely longer than most passwords used in practice 1234567890!@#$%^&*()",
                "zażółć gęślą jaźń"
            ];

            [TestCaseSource(nameof(ValidPasswords))]
            public void VerifyPassword_CorrectPassword_ReturnsTrue(string password)
            {
                var hash = _target.HashPassword(password);

                Assert.That(_target.VerifyPassword(password, hash), Is.True);
            }

            private static readonly (string Password, string Wrong)[] WrongPasswordCases =
            [
                ("password",  "Password"),
                ("password",  "password "),
                ("P@ssw0rd!", "p@ssw0rd!"),
                ("secret",    "secre"),
                ("secret",    "secret1")
            ];

            [TestCaseSource(nameof(WrongPasswordCases))]
            public void VerifyPassword_WrongPassword_ReturnsFalse((string password, string wrong) tc)
            {
                var hash = _target.HashPassword(tc.password);

                Assert.That(_target.VerifyPassword(tc.wrong, hash), Is.False);
            }

            [TestCase("password")]
            [TestCase("P@ssw0rd!")]
            public void VerifyPassword_TamperedHash_ReturnsFalse(string password)
            {
                var hash = _target.HashPassword(password);

                var bytes        = Convert.FromBase64String(hash);
                bytes[SaltSize] ^= 0xFF;
                var tamperedHash = Convert.ToBase64String(bytes);
                Assert.That(_target.VerifyPassword(password, tamperedHash), Is.False);
            }

            [TestCase("password")]
            public void VerifyPassword_TamperedSalt_ReturnsFalse(string password)
            {
                var hash         = _target.HashPassword(password);
                var bytes        = Convert.FromBase64String(hash);
                bytes[0] ^= 0xFF;
                var tamperedHash = Convert.ToBase64String(bytes);

                Assert.That(_target.VerifyPassword(password, tamperedHash), Is.False);
            }
        }
    }
}