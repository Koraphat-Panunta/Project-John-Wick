using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmoredProtectionAttachSocket : MonoBehaviour
{
    [System.Serializable]
    public struct ArmoredSocket
    {
        public BodyPart bodyPart;
        public Armored_Protection armored_Protection;
    }

    [SerializeField] SkinnedMeshRenderer characterskinMeshRenderer;

    public ArmoredSocket[] armoredSockets;
    public FullBodyCharacterPart fullBodyCharacterPart;
    public void UpdateArmoredSocket()
    {
        if(this.armoredSockets == null
            || this.armoredSockets.Length <= 0)
            return;

        for (int i = 0; i < armoredSockets.Length; i++) 
        {
            if (this.armoredSockets[i].armored_Protection == null)
                continue;

            switch (this.armoredSockets[i].armored_Protection)
            {
                case BodyArmored_Protection bodyArmored_Protection:
                    {
                        bodyArmored_Protection.Attach(this.armoredSockets[i].bodyPart, this.characterskinMeshRenderer);
                    }
                    break;
                case Armored_Protection armored_Protection:
                    {
                        armored_Protection.Attach(this.armoredSockets[i].bodyPart);
                    }break;
                
            }

            SaveEditorChanged.SaveEditorChangedObject(this.armoredSockets[i].armored_Protection);
        }
    }

    public void SetupFullBodyCharacterPart()
    {
        this.armoredSockets = new ArmoredSocket[11];

        this.armoredSockets[0] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.headBodyPart
        };

        this.armoredSockets[1] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.hipBodyPart
        };
        this.armoredSockets[2] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.spline_0BodyPart
        };

        this.armoredSockets[3] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.armLeftBodyPart
        };
        this.armoredSockets[4] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.foreArmLeftBodyPart
        };

        this.armoredSockets[5] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.armRightBodyPart
        };
        this.armoredSockets[6] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.foreArmRightBodyPart
        };

        this.armoredSockets[7] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.lowerLegLeftBodyPart
        };
        this.armoredSockets[8] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.upperLegLeftBodyPart
        };

        this.armoredSockets[9] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.lowerLegRightBodyPart
        };
        this.armoredSockets[10] = new ArmoredSocket()
        {
            bodyPart = this.fullBodyCharacterPart.upperLegRightBodyPart
        };
    }
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(ArmoredProtectionAttachSocket))]
public class ArmoredProtectionAttachSocketEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("UpdateArmoredSocket"))
        {
            var setup = (ArmoredProtectionAttachSocket)target;
            setup.UpdateArmoredSocket();


        }

        if (GUILayout.Button("SetupFullBodyCharacterPart"))
        {
            var setup = (ArmoredProtectionAttachSocket)target;
            setup.SetupFullBodyCharacterPart();


        }

        DrawDefaultInspector();
    }
}

#endif


