#if UNITY_LOCALIZATION_INSTALLED
using UnityEngine;
using UnityEngine.Localization;

namespace Rascar.Toolbox.Extensions
{
    public class LocalizedStringExtensions
    {
        public static string GetLocalizedStringOrEmpty(this LocalizedString localizedString)
        {
            if (localizedString.TableReference.ReferenceType == UnityEngine.Localization.Tables.TableReference.Type.Empty
                || localizedString.TableEntryReference.ReferenceType == UnityEngine.Localization.Tables.TableEntryReference.Type.IsEmpty)
            {
                return string.Empty;
            }

            return localizedString.GetLocalizedString();
        }

        public static bool IsNullOrEmpty(this LocalizedString localizedString)
        {
            return localizedString == null || localizedString.IsEmpty;
        }

        public static LocalizedString Copy(this LocalizedString localizedString)
        {
            return new LocalizedString(localizedString.TableReference, localizedString.TableEntryReference);
        }
    }
}
#endif
