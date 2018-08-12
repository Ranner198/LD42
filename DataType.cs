using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataType {

    public string name;
    public int size;
    public Color color;

    public DataType(string name) {

        this.name = name;

        GameObject temp = new GameObject();

        temp.name = this.name;

        if (name == "char") { this.size = 1; this.color = Color.yellow; }
        else if (name == "int") { this.size = 2; this.color = Color.red; }
        else if (name == "float") { this.size = 4; this.color = Color.green; }
        else if (name == "double") { this.size = 8; this.color = Color.blue; }
    }

    public string getName() {
        return this.name;
    }

    public Color getColor() {
        return this.color;
    }
}
