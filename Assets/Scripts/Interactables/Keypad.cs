using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// I AM GOING TO EXPLODE YOU IF DON'T UNDERSTAND THIS CODE <br/>
/// <br/>
/// #####%%%%%%%%%%%###*****************+*+++++++++++++++++++==================+++++++++++++++**+**<br/>
/// ######%%%%%%%%%%%%%%%%###**************+++++++++++++======----=-===============+++++++++++++***<br/>
/// ########%%##%%%%##%%%%%%%%#####**********+++++++++++===-------------================+++++++++**<br/>
/// ########################%%%####%####********++**#%@@@@@%+-------------=============+++++++++***<br/>
/// #########################################***#@@@@@@@@@@@@@*=----------------===##++++++++++****<br/>
/// ###########################################%%%@%%@@@@@@@@@@@@%+--------------==+#**#@@%*++*****<br/>
/// ##################################****##*++###*#%%####%%@@@@@@@*+--------------=+=====+*@@*****<br/>
/// ################################*+===*=:.:++--*#=--+******##%@@@@@@#=-----------+=======#@++***<br/>
/// #############################***+=:-:.-=:*::-*+:-+*+=-=====+*#%%@@@@@=---------=#*======#@++***<br/>
/// ############################****++*=+###%*++*++*******=:---. :=*%%@@@@=--------=**======+*++***<br/>
/// %%%##%%#####################**#*#%%%%%%#########****+++++**++=  -*%%%@*. :----+-*=+-=====++****<br/>
/// %%%%%%%###################*****@@@@%%####%######*************###*=#%%%@--#=--*%+#=-=====#%****#<br/>
/// %%%%%%%%%%%%%%#############***%@@@%****#####***+++=++*****####%%%@%%@@@+ :%+*+@@@@@**++@@#*####<br/>
/// %%%%%%%%%%%%#############****+*@@%**+=+***+++==-----===+++**##%%%@@@@@@+:+#@@@@@@@@@@@@@%##%%%%<br/>
/// @@@%%%%%%%%%%%############***==@@#++-::::----::::.....:::-=+**#%%%%%@@%%#*%@@@%%@%@@@@%%%%%@@@@<br/>
/// %%@@%%%%%%%%%%%%##########***==*%+=--::::....   ..:::::::...=+#%%#%%%%- .%@@@%#%@@@#@@@%%@@@@@@<br/>
/// #%@@@@@%%%%%%%%%%###########*= -*+=------====--:::::--------=+#######*..=#@*#**%@@@@@@@@%@@@@@@<br/>
/// *%@@@@%%%%%%%%%%%%@@@@@@@@@%#+-*+=+====++++===-:::---========*####*+++ +@@%##*%@@@@@@@##@@@@@@@<br/>
/// @@@@@@%%%%%%%%%@@@@@@@@@@@@@#**+=+*++=----===-:::::...::::-=*#####*++:  =@%**%@@@@@@@@#%@@@@@@@<br/>
/// %%%%%%%%%%%%%@@@@@@@@@@@@%#=:==--++=**+---::..  ..      .:=*##***#**+- +##@**%@@@@@@@@#%@@@@@@@<br/>
/// %%%%%%%%%%%%@@@@@@@@@@@%#*:  :*:-=--+***+=.            .=+++-::-=*###-  +===#@@%%@@@@#*#@@@@@@@<br/>
/// %%%%%%%%%%%%@@@@@@@@%####+. +####+=----==-----...  ::---:-===----+*###. *%=:=#%%#@#*+++*%%@@@@@<br/>
/// %%%%%%%%%%%%@@@@@@########%@@%#**=:.==*#%+.  :-------:   =##=:++-=+***: @@=:-+:-==---=++#%@%#%%<br/>
/// @@@%%%%%%###%@@@#%%%####@@@@@@#*+::=:-=++----=+++==+**=::. .:.-+++**=:.  .+:=-:=++---===*%*+**#<br/>
/// @@@@%%%%##%%%%%%%%%%%#%@@@@@@@%:+-:..      .=*+++++*+**=:...:=-=***+..    --+-#@%#---=*%**+++**<br/>
/// @@@%%#%%%%%%%%%%%%%%%%%@@@@@@@%:+=:. :-:.:=+++++++++*++=.    .:-=+++-    .=:-==-+*-----+*@@@@#*<br/>
/// @@% *#######%%%%%%%%%#####%@@@@@*=::.... :-....++=++**+.  .::..::--=*:    :=:-=:-+*------++==+*<br/>
/// %#*++=-::.:=+*##%%##%##=..:*###*:  .:::::.. ..=**+***+-.    ... ..     =  --=-:-**---=+=++===+*<br/>
/// *%%#*=.       .-*******+  .-:-*#*            =+++==+**+          :.:**=@+*#+-:-%@%+-+@=-+====+*<br/>
/// %%#*-              ..  .  .   #@@@+            .::..:: .     .. .. .   -..*-::-%@@@-+#==*+==+*#<br/>
/// #*-.     :=++-:             .%@@@@%       ..::       .::::   . .:: .   *+=*-::--*%+-===+++===*#<br/>
/// =:      .= *++-            =@@@@@@@@.      ..:::     .:::...  . .:--- :. ==+= --#%%@@@@#*#*++*#<br/>
/// .       .--::.       .  .%@@@@@@@@@*.... :-= -.   .  =::-=#@@@+-=+==+=++**                     <br/>
///       ...          :.   +@@@@@@@@@@%= ..-@####*-. .=:--==*###===+*#@%+**                       <br/>
///          :-:      :.   %@@@@@@@@@@@@@=     :::---------:..    :+@@++*#-...=:--==+*+======*++++*<br/>
///                      .%@@@@@@@@@@@@@@@                      ..-+@@@+...= --=== +*++++=== +#+++*<br/>
///                .@@@%%@@@@@@@@@@@@@@@@@:          .:         ..-+@@@@@  ...= ----==== +++**++*++<br/>
///            :+%@@@@@@@@@@@@@@@@@@@@@@@@#       ..    ....     .-#@@@@@@-...=----=======++*##*#%%<br/>
///      :*%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@-.. =@@@@@@@@@=.= --======= +%@@*#**#%#                 <br/>
///  +%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#:                   .*@@@@@@@@@@@@@%========++++**#****<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%:  .              .#@@@@@@@@@@@@@@@@@===++++++++******<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%: .             -%@@@@@@@@@@@@@@@@@@@@*+++++++++*****<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#.             =%@@@@@@@@@@@@@@@@@@@@@@@@@@%*********<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%:            +@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@=.         .#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@% -     .= *#%%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%=  :#%%%%%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%##%%%%%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%#%%%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@% =%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@<br/>
/// @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@<br/>
/// 
/// </summary>
public class Keypad : MonoBehaviour, IInteractable
{
    public GameObject keypad;
    public TMP_Text displayText;
    public Button[] numberButtons;
    public Button enterButton;

    public NPCDialogue malumaLines;

    private bool isKeypadActive;
    private string currentCombination;
    private int correctAttempts;
    
    private DialogueController dialogueUI;
    private DialogueLine[] dialogueLines;
    private int dialogueIndex;
    private bool isTyping;
    private bool malumaActive = false;



    void Start()
    {
        foreach (Button btn in numberButtons)
        {
            btn.onClick.AddListener(() => OnNumberPressed(btn));
        }

        enterButton.onClick.AddListener(() => OnEnterPressed());
        displayText.text = "";
        
        dialogueUI = DialogueController.instance;
    }

    public bool CanInteract()
    {
        return !isKeypadActive;
    }

    public void Interact()
    {
        if(!malumaActive){
            isKeypadActive = true;
            PauseManager.IsPaused = isKeypadActive;
            keypad.SetActive(true);
        }
        else
        {
            NextDialogueLine(); 
        }
    }

    public void CloseKeypad()
    {
        isKeypadActive = false;
        currentCombination = "";
        PauseManager.IsPaused = isKeypadActive;
        keypad.SetActive(false);
    }

    public void OnNumberPressed(Button btn)
    {
        if (displayText.text.Length < 4)
        {
            displayText.text += btn.name;
            currentCombination += btn.name;
        }
    }

    void OnEnterPressed()
    {
        if (currentCombination == "0000" && correctAttempts < 6)
        {
            displayText.text = "TRY AGAIN";
            correctAttempts++;
            StartCoroutine(ClearScreen());
        }
        else if (correctAttempts == 6)
        {
            displayText.text = "CORRECT";
            malumaActive = true;
            StartCoroutine(OpenMalumaDialogue());
        }
        else
        {
            displayText.text = "INCORRECT";
            StartCoroutine(ClearScreen());
        }
    }

    IEnumerator ClearScreen()
    {
        yield return new WaitForSeconds(0.7f);
        displayText.text = "";
        currentCombination = "";
    }
    
    IEnumerator OpenMalumaDialogue()
    {
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(ScreenFader.instance.FadeOut());


        keypad.SetActive(false);
        keypad.SetActive(false);
        isKeypadActive = false;
        currentCombination = "";

        dialogueLines = malumaLines.dialogueLines;
        dialogueIndex = 0;

        Sprite portrait = malumaLines.npcPortraitSprites[dialogueLines[0].portraitIndex];
        dialogueUI.SetNPCInfo(malumaLines.npcName, portrait);
        dialogueUI.ShowDialogueUI(true);

        DisplayCurrentLine();
    }

    public void NextDialogueLine()
    {
        if (!dialogueUI.gameObject.activeSelf) return;

        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueLines[dialogueIndex].text);
            isTyping = false;
            return;
        }

        if (++dialogueIndex < dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndMalumaDialogue();
        }
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach (char c in dialogueLines[dialogueIndex].text)
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += c);
            yield return new WaitForSeconds(malumaLines.typingSpeed);
        }

        isTyping = false;
    }

    void EndMalumaDialogue()
    {
        StopAllCoroutines();
        isTyping = false;
        PauseManager.IsPaused = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        
        // End Sequence
        StartCoroutine(EndSequence());
    }
    
    private IEnumerator EndSequence()
    {
        yield return StartCoroutine(ScreenFader.instance.FadeIn());

        yield return new WaitForSeconds(0.5f);
        
        DeleteAllPersistentObjects();
        AsyncOperation load = SceneManager.LoadSceneAsync("TitleScreen");
        
        while (!load.isDone) yield return null;

        yield return StartCoroutine(ScreenFader.instance.FadeOut());
    }

    public void DeleteAllPersistentObjects()
    {
        Scene[] allScenes = SceneManager.GetAllScenes();
        
        foreach (Scene scene in allScenes)
        {
            if (scene.name == "DontDestroyOnLoad")
            {
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    Destroy(obj);
                }
            }
        }
    }
}