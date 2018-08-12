using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Stack : MonoBehaviour
{

    public int LvlSize;

    private int BugIndex;

    public int[] StackSize;

    public DataType[] VisualStack;

    private bool changed = true;

    public GameObject cubes;

    //UI Elements
    public InputField input;
    public Text output;

    public int currentIndex = 0;

    DataType dataChar, dataInt, dataFloat, dataDouble, temp;

    void Start()
    {
        //instiate the arrays, and allocate memory
        StackSize = new int[LvlSize];
        VisualStack = new DataType[LvlSize];

        //Add bug
        BugIndex = Randomizer(LvlSize);

        //get the right width
        Redraw();

        //Get UI components
        input = GameObject.Find("InputField").GetComponent<InputField>();
        output = GameObject.Find("Output").GetComponent<Text>();
    }

    void Update()
    {

        //if variables are null --------------------------------------------------------
        if (input == null)
            input = GameObject.Find("InputField").GetComponent<InputField>();
        if (output == null)
            output = GameObject.Find("Output").GetComponent<Text>();
        //------------------------------------------------------------------------------

        //Treverse the array -----------------------------------------------------------
        if (!input.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                DestroyAll();
                currentIndex--;
                changed = true;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                DestroyAll();
                currentIndex++;
                changed = true;
            }
        }
        //No break pls
        if (currentIndex > LvlSize - 1)
            currentIndex = 0;
        else if (currentIndex < 0)
            currentIndex = LvlSize - 1;
        //------------------------------------------------------------------------------

        //Run input code
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Enter();
        }

        //Draw Array System
        if (changed)
        {
            float counter = 0;
            changed = false;

            for (int i = 0; i < StackSize.Length; i++)
            {
                if (i == BugIndex)
                {
                    if (DrawBug())
                    {
                        GameObject[] all = GameObject.FindGameObjectsWithTag("Bug");
                        for (int d = 0; d < all.Length; d++)
                        {
                            Destroy(all[d]);
                        }

                        //Draw Bug
                        GameObject StackObject = Instantiate(cubes, gameObject.transform.position, Quaternion.identity);
                        StackObject.name = "Bug";
                        StackObject.tag = "StackObject";
                        StackObject.transform.parent = gameObject.transform;
                        StackObject.transform.position += new Vector3(i + ((counter + 1) / 10), 0, 9.8f);
                        StackObject.tag = "Bug";
                        StackObject.GetComponent<MeshRenderer>().material.color = Color.black;
                    }
                }

                //If nothing
                if (VisualStack[i] == null)
                {
                    counter++;
                    GameObject StackObject = Instantiate(cubes, gameObject.transform.position, Quaternion.identity);
                    StackObject.name = "Stack_Object_" + i;
                    StackObject.tag = "StackObject";
                    StackObject.transform.parent = gameObject.transform;
                    StackObject.transform.position += new Vector3(i + (counter / 10), 0, 10);
                }

                //If there is something in this index
                if (VisualStack[i] != null)
                {
                    counter++;
                    GameObject StackObject = Instantiate(cubes, gameObject.transform.position, Quaternion.identity);
                    StackObject.name = "Stack_Object_" + i;
                    StackObject.tag = "StackObject";
                    StackObject.transform.parent = gameObject.transform;
                    StackObject.transform.position += new Vector3(i + (counter / 10), 0, 10);
                    StackObject.GetComponent<MeshRenderer>().material.color = VisualStack[i].getColor();
                }

                //Selected Index
                if (currentIndex == i)
                {
                    GameObject StackObject = Instantiate(cubes, gameObject.transform.position, Quaternion.identity);
                    StackObject.name = "Cursor";
                    StackObject.tag = "StackObject";
                    StackObject.transform.parent = gameObject.transform;
                    StackObject.transform.position += new Vector3(i + (counter / 10), 0, 10.5f);
                    StackObject.GetComponent<MeshRenderer>().material.color = Color.white;
                    //Scale Obj
                    var size = StackObject.transform.localScale;
                    size += new Vector3(.25f, .25f, .25f);
                    StackObject.transform.localScale = size;
                }
            }
            counter = 0;
        }
    }

    void push()
    {
        DestroyAll();
        changed = true;
    }

    void pop()
    {
        DestroyAll();
        changed = true;
    }

    void Redraw()
    {
        int offset = LvlSize;
        Vector3 Holder = new Vector3(transform.position.x, 0, 0);
        Holder.x -= offset * .475f;
        //Holder.x -= offset;
        transform.position = Holder;
    }

    void Enter()
    {

        //do a include statement here to decide which function
        var m_String = input.GetComponent<InputField>().text.ToLower();

        bool isSafe = true;

        //Char assertion --------------------------------------------------------------------------------------------
        dataChar = new DataType("char");
        if (m_String == "stack.push(char);" || m_String == "stack.push(char)")
        {
            if (currentIndex == 0 || VisualStack[currentIndex - 1].getName() != dataChar.getName())
            {
                if (VisualStack[currentIndex] == null)
                {
                    VisualStack[currentIndex] = dataChar;
                    push();
                }
                else
                    output.text = "Error: Position already filled, try pop() to free the area :(".ToString();
            }
            else
                output.text = "Error: You are not allowed to use the same data type twice in a row :(".ToString();
        }
        //Int assertion --------------------------------------------------------------------------------------------
        else if (m_String == "stack.push(int);" || m_String == "stack.push(int)")
        {
            temp = new DataType("int");
            if (dataInt.size > LvlSize - currentIndex)
                output.text = "Error: There is not enough room left to push this data type try a smaller one :)".ToString();
            if (temp.size > LvlSize - currentIndex)
                output.text = "Error: There is not enough room left to push this data type try a smaller one :)".ToString();
            for (int i = 0; i < temp.size; i++)
            {
                if (VisualStack[i + currentIndex] != null)
                {
                    output.text = "Error: There is not enough room to push this data type into the Stack :(".ToString();
                    isSafe = false;
                    return;
                }
            }
            if (isSafe)
            {
                dataInt = new DataType("int");
                for (int i = 0; i < dataInt.size; i++)
                {
                    VisualStack[i + currentIndex] = dataInt;
                }
                push();
            }
        }
        //Float assertion --------------------------------------------------------------------------------------------
        else if (m_String == "stack.push(float);" || m_String == "stack.push(float)")
        {
            temp = new DataType("float");
            if (temp.size > LvlSize - currentIndex)
                output.text = "Error: There is not enough room left to push this data type try a smaller one :)".ToString();
            for (int i = 0; i < temp.size; i++)
            {
                if (VisualStack[i + currentIndex] != null)
                {
                    output.text = "Error: There is not enough room to push this data type into the Stack :(".ToString();
                    isSafe = false;
                    return;
                }
            }
            if (isSafe)
            {
                dataFloat = new DataType("float");
                for (int i = 0; i < dataFloat.size; i++)
                {
                    VisualStack[i + currentIndex] = dataFloat;
                }
                push();
            }
        }
        //Double assertion --------------------------------------------------------------------------------------------
        else if (m_String == "stack.push(double);" || m_String == "stack.push(double)")
        {
            temp = new DataType("double");
            if (temp.size > LvlSize - currentIndex)
                output.text = "Error: There is not enough room left to push this data type try a smaller one :)".ToString();
            for (int i = 0; i < temp.size; i++)
            {
                if (VisualStack[i + currentIndex] != null)
                {
                    output.text = "Error: There is not enough room to push this data type into the Stack :(".ToString();
                    isSafe = false;
                    return;
                }
            }
            if (isSafe)
            {
                dataDouble = new DataType("double");
                for (int i = 0; i < dataDouble.size; i++)
                {
                    VisualStack[i + currentIndex] = dataDouble;
                }
                push();
            }
        }

        temp = null;
    }

    void DestroyAll()
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("StackObject");
        for (int i = 0; i < all.Length; i++)
        {
            Destroy(all[i]);
        }
    }

    int Randomizer(int lvlSize)
    {
        int x = Random.Range(0, lvlSize - 1);
        return x;
    }

    bool DrawBug() {
        //See if something was added on top if so move over to the right
        for (int x = 0; x < LvlSize; x++)
        {
            if (BugIndex >= LvlSize)
            {
                output.text = "Good Job! you removed the bug! ready for another challenge? Stack.yes or Stack.no";
                return false;
            }

            if (VisualStack[x] == null && x == BugIndex)
                return true;
            else if (VisualStack[x] == null && x >= BugIndex)
                return false;
            else if (VisualStack[x] != null && x == BugIndex)
                BugIndex++;
        }

        return false;
    }
}
