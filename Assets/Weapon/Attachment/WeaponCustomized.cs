using UnityEngine;

public class WeaponCustomized : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Weapon Weapon;
    [SerializeField] private Sight sight;
    [SerializeField] private Muzzle Silencer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //if (sight.isActiveAndEnabled == false)
            //    Instantiate(sight);
            sight.Attach(Weapon);
        }
    }
    private void OnGUI()
    {
        //if(GUILayout.Button("Attach Sight"))
        //{
           
        //}
        //if(GUILayout.Button("Attach Silencer"))
        //{
        //    if (Silencer.isActiveAndEnabled == false)
        //        Instantiate(Silencer);
        //    Silencer.Attach(player.currentWeapon);
        //}
    }
}
