using UnityEngine;

public static class HPDamageBeahavior 
{
    public static float CheckDamageAfterTaken(IHPDamageVisitor hPDamageVisitor,Character character)
    {
        return character.GetHP() - hPDamageVisitor._hPDamage;
    }
}
