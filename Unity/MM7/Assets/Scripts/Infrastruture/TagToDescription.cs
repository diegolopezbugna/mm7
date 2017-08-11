using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Business {

public static class String {

    public static string TagToDescription(this string tag) {
        switch (tag)
        {
            case "EnemyDragonfly":
                return "Dragonfly";
        }

        return tag;
    }

}

}
