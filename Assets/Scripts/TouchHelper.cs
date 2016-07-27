using UnityEngine;
using System.Collections;

public enum SwipeType
{
    None = 0,
    Left,
    Right,
    Up,
    Down
};

public class TouchHelper
{
    private float startTime;
    private Vector2 startPos;
    private bool couldBeSwipe;

    private float minSwipeDistX;
    private float minSwipeDistY;
    private float maxSwipeTime;
    private Touch touch;
    private bool waitForEnd;
    private bool isSingle;

    public TouchHelper(float minSwipeDistX = 300f, float minSwipeDistY = 300f, float maxSwipeTime = 2f, bool waitForEnd = true, bool isSingle = true)
    {
        this.minSwipeDistX = minSwipeDistX;
        this.minSwipeDistY = minSwipeDistY;
        this.maxSwipeTime = maxSwipeTime;
        this.waitForEnd = waitForEnd;
        this.isSingle = isSingle;
    }

    public SwipeType Detect()
    {
        if (Input.touches.Length > 0)
        {
            touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    couldBeSwipe = true;
                    startPos = touch.position;
                    startTime = Time.time;
                    break;

                case TouchPhase.Moved:

                    if (!waitForEnd)
                        return SwipeEnded();

                    break;

                case TouchPhase.Ended:
                    if (waitForEnd)
                        return SwipeEnded();
                    couldBeSwipe = false;

                    break;
            }
        }
        return SwipeType.None;
    }

    private SwipeType SwipeEnded()
    {
        var swipeTime = Time.time - startTime;
        var swipeDistX = (touch.position.x - startPos.x);
        var swipeDistY = (touch.position.y - startPos.y);

        if (couldBeSwipe && (swipeTime < maxSwipeTime))
        {

            if (Mathf.Abs(swipeDistX) > Mathf.Abs(swipeDistY) && Mathf.Abs(swipeDistX) > minSwipeDistX)
            {
                float swipeDirectionX = Mathf.Sign(touch.position.x - startPos.x);
                //couldBeSwipe = false;

                if (couldBeSwipe = !isSingle)
                {
                    startPos = touch.position;
                    startTime = Time.time;
                }

                if (swipeDirectionX > 0)
                {
                    return SwipeType.Right;
                }
                else
                {
                    return SwipeType.Left;
                }
            }
            else if (Mathf.Abs(swipeDistY) > Mathf.Abs(swipeDistX) && Mathf.Abs(swipeDistY) > minSwipeDistY)
            {

                float swipeDirectionY = Mathf.Sign(touch.position.y - startPos.y);
                if (couldBeSwipe = !isSingle)
                {
                    startPos = touch.position;
                    startTime = Time.time;
                }

                if (swipeDirectionY > 0)
                {
                    return SwipeType.Up;
                }
                else
                {
                    return SwipeType.Down;
                }

            }
            //couldBeSwipe = false;
        }
        return SwipeType.None;
    }
}

