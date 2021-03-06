﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebugInput : MonoBehaviour {

    private List<CharController> npcs = new List<CharController>();
    private int npc_index = -1;

    public Text Name;
    public Text Health;
    public Text Ammo;
    public Text Behaviour;
    public Toggle Draw_Path;

	// Use this for initialization
	void Start () {
        var list = GameObject.FindObjectsOfType < CharController>();
        foreach (var ch in list)
        {
			if (ch.char_type == CharController.CharType.NPC || ch.char_type == CharController.CharType.NPC_Melee) npcs.Add(ch);
        }
        nextNPC();
	}

    public void nextNPC()
    {
        if (npcs.Count <= 0) return;

		npc_index = (npc_index+1)%npcs.Count;

        if (npcs[npc_index] == null)
        {
            npcs.RemoveAt(npc_index);
            nextNPC();
        }
        else
        {
            Draw_Path.onValueChanged.RemoveAllListeners();
			Draw_Path.isOn = npcs [npc_index].show_path;
            Draw_Path.onValueChanged.AddListener(npcs[npc_index].set_show_path);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (npcs.Count <= 0 || npcs[npc_index] == null || npc_index > npcs.Count) return;


		Name.text = npcs[npc_index].gameObject.name;
        Health.text = "Health: "+ npcs[npc_index].health+"/"+npcs[npc_index].health_max;
        Ammo.text = "Ammo: " + npcs[npc_index].ammo + "/" + npcs[npc_index].ammo_max;
        Behaviour.text = "Behaviour: " + npcs[npc_index].current_behavoiur();
	
	}
}
