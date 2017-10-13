using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infrastructure;

namespace Business {

    public static class String {

        public static string TagToDescription(this string tag) {
            return Localization.Instance.Get(tag.StartsWith("Enemy") ? tag.Substring(5) : tag);
        }

    }

}
