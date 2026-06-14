using UnityEngine;
using UnityEngine.InputSystem;

public class Ring : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _action;
    private InputAction _toss;
    private InputAction _drag;

    private float _minDistance;
    private float _maxDistance;

    /**
     *  
     *  isMouseDown = false
     *  mousePosition
     *  
     *  isMouseClicked = false
     *  
     *  if (isMouseDown == true) {
     *      if (isMouseClicked == true) {
     *          return
     *      } else {
     *          start = mousePosition
     *          isMouseClicked = true
     *      }
     *  } else {
     *      if (isMouseClicked == true) {
     *          isMouseClicked = false
     *          end = mousePosition
     *          
     *          diff = end - start
     *          
     *          if (diff >= _minDistance and diff <= _maxDistance) {
     *              Shoot(diff)
     *          }
     *      }
     *  }
     *  
     *  
     *  
     *  /

    /** 
     * isMouseDown = false
     * mousePosition
     * 
     * 
     * 
     * def DragDistance():
     * 
     * start = 0
     * end  = 0
     * dif = 0
     * 
     * if isMouseDown == true:
     *      start = mousePosition;
     *      
     *      else:
     *      isMouseDown == false:
     *          start != mousePosition 
     *      
     * if end > 0:
     *      dif = start - end;
     *      
     * 
     */

    // To get the distance of a drag, you would first determine it's starting "drag location" and look at the "end drag location" and once you have both numbers, you would 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _toss = _action.FindActionMap("RingToss").FindAction("Toss");
        _drag = _action.FindActionMap("RingToss").FindAction("Drag");


    }

    // Update is called once per frame
    void Update()
    {
        // Check wether or not the range of movement is within a valid range with it being from 1 - 10 (for example). If it is less, then nothing happends and if it's over then be like
        // "You threw too hard"
    }

    private void OnEnable()
    {
        _toss.Enable();
        _drag.Enable();
    }

    private void OnDisable()
    {
        _toss.Disable();
        _drag.Disable();
    }
}
