using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ObjectPooler objectPooler;

    private float prefabHeight;
    private float bldgHeight;
    public List<GameObject> bldgs;

    public int colour;

    void Start() {
        prefabHeight = 1.92f;
        objectPooler = GetComponent<ObjectPooler>();
        reset(true);
    }

    public void reset(bool start = false) {
        foreach (GameObject obj in bldgs)
        {
            obj.SetActive(false);
        }
        bldgs = new List<GameObject>();

        if (!start) { selectColour(); }

        bldgHeight = -2.05f;

        for(int i = 0; i < 8; i++) {
            bldgs.Add(objectPooler.spawnFromPool(bldgHeight, colour));
            bldgHeight += prefabHeight;
        }

    }

    public void removeFirstBldg(int score)
    {
        bldgs.Add(objectPooler.spawnFromPool(bldgHeight, colour));
        bldgHeight += prefabHeight;
    }

    public string getDirectionFirstBldg(int score)
    {
        return bldgs[score].tag.ToString();
    }

    public void selectColour() {
        switch (colour) {
            case 0:
            case 1:
                colour += 1;
                break;
            case 2:
                colour = 0;
                break;
        }
    }
}

