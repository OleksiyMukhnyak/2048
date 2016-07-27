using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameHelper : MonoBehaviour
{
    public GameObject GameMenu;
    public GameObject MainMenu;
    public GameObject GameField;
    public GameObject GameOverField;
    public Text TScore;
    public Text THightScore;

    public Level CurrentLvl { get { return _currentLvl; } }
    public bool CanMove { get { return _canMove; } }

    private bool _canMove;

    private TouchHelper _touchControl;
    private SwipeType _swipeType;

    private LevelHelper _lvlHelper;
    private Level _currentLvl;
    private ItemStyle _itemStyle;

    private SquareHelper[,] _squares;
    private List<SquareHelper[]> _colSquares;
    private List<SquareHelper[]> _rowSquares;
    private List<SquareHelper> _emptySquares;

    void Awake()
    {
        _touchControl = new TouchHelper(50, 50, 2, false, true);
        _swipeType = SwipeType.None;
        _canMove = false;
        _lvlHelper = GetComponent<LevelHelper>();
        _itemStyle = GetComponent<ItemStyle>();
    }

    void Start()
    {
        AdsHelper.Instance.Init();
        AdsHelper.Instance.showBanner();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && CanMove)
            Move(SwipeType.Left);
        if (Input.GetKeyDown(KeyCode.W) && CanMove)
            Move(SwipeType.Up);
        if (Input.GetKeyDown(KeyCode.D) && CanMove)
            Move(SwipeType.Right);
        if (Input.GetKeyDown(KeyCode.S) && CanMove)
            Move(SwipeType.Down);

        if (!CanMove & Input.touchCount == 0)
            return;

        _swipeType = _touchControl.Detect();

        Move(_swipeType);
    }

    public void StartGame()
    {
        GameMenu.SetActive(true);
        MainMenu.SetActive(false);
        GameOverField.SetActive(false);

        _currentLvl = _lvlHelper.CurrentLvl;
        CalculationSquareSize();

        _squares = new SquareHelper[_currentLvl.height, _currentLvl.weight];
        _emptySquares = new List<SquareHelper>();

        for (int i = 0; i < _currentLvl.height; i++)
        {
            for (int j = 0; j < _currentLvl.weight; j++)
            {
                SquareHelper squeare = Instantiate(Resources.Load<SquareHelper>("Prefabs/Square"));
                squeare.transform.SetParent(GameField.transform, false);
                squeare.AddItem(_itemStyle.AllItemStyle[0]);
                squeare.row = i;
                squeare.col = j;
                _squares[i, j] = squeare;
                _emptySquares.Add(squeare);
            }
        }

        _colSquares = new List<SquareHelper[]>();
        _rowSquares = new List<SquareHelper[]>();


        SquareHelper[] rows;
        SquareHelper[] cols;

        for (int i = 0; i < _currentLvl.height; i++)
        {
            rows = new SquareHelper[_currentLvl.weight];
            cols = new SquareHelper[_currentLvl.height];

            for (int j = 0; j < _currentLvl.weight; j++)
            {
                cols[j] = _squares[j, i]; //перевірити чи я не напутав
                rows[j] = _squares[i, j];
            }

            _colSquares.Add(cols);
            _rowSquares.Add(rows);
        }


        //foreach(var col in _colSquares)
        //{
        //    for (int i = 0; i < col.Length; i++)
        //    {
        //        Debug.Log("col [" + col[i].col + "][" + col[i].row);         
        //    }   
        //}

        Generate();
        Generate();

        _canMove = true;
    }

    public void GameOver()
    {
        AdsHelper.Instance.showInterstitial();

        _canMove = false;

        GameOverField.SetActive(true);
    }

    public void GameWin()
    {

    }

    public void RestartGame()
    {
        for (int i = 0; i < _currentLvl.height; i++)
        {
            for (int j = 0; j < _currentLvl.weight; j++)
            {
                Destroy(_squares[i, j].gameObject);
            }
        }
    }

    public void ExitToMainMenu()
    {
        GameMenu.SetActive(false);
        MainMenu.SetActive(true);
        GameOver();
        RestartGame();
    }

    void CalculationSquareSize()
    {
        GridLayoutGroup grid = GameField.GetComponent<GridLayoutGroup>();
        RectTransform rectTrans = GameField.GetComponent<RectTransform>();

        int iRow = _currentLvl.height;
        int iColumn = _currentLvl.weight;

        float fHeight = (rectTrans.rect.height - ((iRow - 1) * (grid.spacing.y))) - ((grid.padding.top + grid.padding.bottom));
        float fWidth = (rectTrans.rect.width - ((iColumn - 1) * (grid.spacing.x))) - ((grid.padding.right + grid.padding.left));

        grid.cellSize = new Vector2(fWidth / iColumn, (fHeight) / iRow); ;
    }

    void Generate()
    {
        if (_emptySquares.Count == 0)
            return;

        float change = Random.Range(0.0f, 100.0f);
        int randomPosition = Random.Range(0, _emptySquares.Count);

        if (change < 90.0f)
        {
            _emptySquares[randomPosition].AddItem(_itemStyle.AllItemStyle[1]);
        }
        else
        {
            _emptySquares[randomPosition].AddItem(_itemStyle.AllItemStyle[2]);
        }

        _emptySquares.Remove(_emptySquares[randomPosition]);
    }

    bool MakeOneMoveDown(SquareHelper[] squaresLine)
    {
        for (int i = 0; i < squaresLine.Length - 1; i++)
        {
            if (squaresLine[i].type == SquareTypes.Empty & squaresLine[i + 1].type == SquareTypes.NoEmpty)
            {
                squaresLine[i].AddItem(squaresLine[i + 1].Item);
                squaresLine[i + 1].AddItem(_itemStyle.AllItemStyle[0]);
                return true;
            }
            if (squaresLine[i].type == SquareTypes.NoEmpty & squaresLine[i].Item.Number == squaresLine[i + 1].Item.Number
                & squaresLine[i].Item.CanMerge == false & squaresLine[i + 1].Item.CanMerge == false)
            {
                squaresLine[i].AddItem(_itemStyle.AllItemStyle[(int)squaresLine[i + 1].Item.Number + 1]);
                squaresLine[i + 1].AddItem(_itemStyle.AllItemStyle[0]);
                squaresLine[i].Item.CanMerge = true;
                return true;
            }
        }
        return false;
    }

    bool MakeOneMoveUp(SquareHelper[] squaresLine)
    {
        for (int i = squaresLine.Length - 1; i > 0; i--)
        {
            if (squaresLine[i].type == SquareTypes.Empty & squaresLine[i - 1].type == SquareTypes.NoEmpty)
            {
                squaresLine[i].AddItem(squaresLine[i - 1].Item);
                squaresLine[i - 1].AddItem(_itemStyle.AllItemStyle[0]);
                return true;
            }
            if (squaresLine[i].type == SquareTypes.NoEmpty & squaresLine[i].Item.Number == squaresLine[i - 1].Item.Number
              & squaresLine[i].Item.CanMerge == false & squaresLine[i - 1].Item.CanMerge == false)
            {
                squaresLine[i].AddItem(_itemStyle.AllItemStyle[(int)squaresLine[i - 1].Item.Number + 1]);
                squaresLine[i - 1].AddItem(_itemStyle.AllItemStyle[0]);
                squaresLine[i].Item.CanMerge = true;
                return true;
            }
        }
        return false;
    }

    void Move(SwipeType swipe)
    {
        ResetMarge();

        bool move = false;
        for (int i = 0; i < _currentLvl.height; i++)
        {
            switch (swipe)
            {
                case SwipeType.Right:
                    //Debug.Log("Right");
                    while (MakeOneMoveUp(_rowSquares[i])) { move = true; }
                    break;
                case SwipeType.Left:
                    //Debug.Log("Left");
                    while (MakeOneMoveDown(_rowSquares[i])) { move = true; }
                    break;
                case SwipeType.Up:
                    //Debug.Log("Up");
                    while (MakeOneMoveDown(_colSquares[i])) { move = true; }
                    break;
                case SwipeType.Down:
                    //Debug.Log("Down");
                    while (MakeOneMoveUp(_colSquares[i])) { move = true; }
                    break;
            }

        }

        if (swipe == SwipeType.None)
            return;
        if (move)
        {
            UpdateEmptySquare();
            Generate();
        }

        if(!CanMoveAI())
        {
            GameOver();
        }
    }

    void ResetMarge()
    {
        foreach (SquareHelper square in _squares)
            square.Item.CanMerge = false;
    }

    void UpdateEmptySquare()
    {
        _emptySquares = new List<SquareHelper>();
        foreach (SquareHelper square in _squares)
        {
            if(square.type == SquareTypes.Empty)
            {
                _emptySquares.Add(square);
            }
        }
    }

    bool CanMoveAI()
    {
        if (_emptySquares.Count > 0)
            return true;
        else
        {
            for (int i = 0; i < _colSquares.Count; i++)
            {
                for (int j = 0; j < _rowSquares.Count - 1; j++)
                {
                    if (_squares[j, i].Item.Number == _squares[j + 1, i].Item.Number)
                        return true;
                }
            }

            for (int i = 0; i < _rowSquares.Count; i++)
            {
                for (int j = 0; j < _colSquares.Count - 1; j++)
                {
                    if (_squares[i, j].Item.Number == _squares[i, j + 1].Item.Number)
                        return true;
                }
            }
        }

        return false;
    }
}
