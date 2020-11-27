using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocietyLinks : MonoBehaviour
{
    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/bibimbap_games/");
    }

    public void OpenFacebook()
    {
        Application.OpenURL("https://www.facebook.com/Bibimbap-108052500833965/?modal=admin_todo_tour");
    }
}
