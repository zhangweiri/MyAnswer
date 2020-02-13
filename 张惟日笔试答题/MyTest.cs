using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTest : MonoBehaviour
{
    private string _s = "castlejoycastlecatjoy";
    private string[] _wordSet = { "joy", "castle", "cat" };

    private List<Rectangle> rectangleList = new List<Rectangle>();
    private int rectangeCount = 10000;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("测试返回："+IsCansplitWord(_s, _wordSet));


        //生成指定数量随机位置随机长宽的矩形；
        for (int i = 0; i < rectangeCount; i++) {
            rectangleList.Add(new Rectangle(new Vector2(Random.Range(0.5f, 0.5f), Random.Range(0.5f, 0.5f)), 
                            new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f)), i ));
        }

        Debug.Log("重合矩形数量: " + GetOverlayRecangles(rectangleList).Count);

    }


    /// <summary>
    /// -----------------第一题-------------------
    /// 编写思路：循环字符串s， 查找是否符合首字母的单词如果符合首字母，对比该单词与字符串s接下来的单词是否吻合，
    /// 不吻合重新对比下一个单词首字母，如果吻合则记录该字符串吻合单词首字母位置作为拆分点，接着跳过拆分完吻合的字符串，
    /// 进入下一个拆分点的字符串循环查找。
    /// 
    /// 选择该方法的理由： 这样做可以用最少的循环次数找出所有单词。
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="wordset"></param>
    /// <returns></returns>
    public bool IsCansplitWord(string s, string[] wordset)
    {
        List<int> splitIndexList = new List<int>();
        for (int i = 0; i < s.Length; i++)
        {
            int splitIndex = -1;
            int matchingWordIndex = -1;
            for (int j = 0; j < wordset.Length; j++) {
                if (s[i] == wordset[j][0]) {
                    splitIndex = i;
                    for (int k = 1; k < wordset[j].Length; k++) {
                        if (splitIndex + k >= s.Length) {
                            splitIndex = -1;
                            break;
                        }
                        if (wordset[j][k] != s[splitIndex + k]) {
                            splitIndex = -1;
                            break;
                        }
                    }
                    if (splitIndex != -1) {
                        matchingWordIndex = j;
                        break;
                    }
                }
            }

            if (splitIndex != -1 && matchingWordIndex != -1)
            {
                splitIndexList.Add(splitIndex);
                i = splitIndex + (wordset[matchingWordIndex].Length - 1);
            }


            if (splitIndex == -1)
            {
                if (splitIndexList.Count > 0)
                {
                    string spritStr = s.Substring(i, s.Length - i);
                    Debug.Log("无法拆分, 拆分到:"+ spritStr+ "找不到匹配单词");
                }
                else {
                    Debug.Log("无法拆分: 无可拆分项");
                }               
                return false;
            }

        }

        if (splitIndexList.Count > 0)
        {
            string spritStr = "";
            for (int i = 0; i < splitIndexList.Count; i++) {
                if (i + 1 < splitIndexList.Count)
                {
                    spritStr += s.Substring(splitIndexList[i], splitIndexList[i + 1] - splitIndexList[i]) + " ";
                }
                else {
                    spritStr += s.Substring(splitIndexList[i], s.Length - splitIndexList[i]) + " ";
                }
            }
            Debug.Log("可以拆分成："+spritStr);
            return true;
        }
        else {
            Debug.Log("无法拆分: 无可拆分项");
            return false;
        }
    }


    /// <summary>
    /// 获取重合矩形的集合
    /// </summary>
    /// <param name="rectangles"></param>
    /// <returns></returns>
    public List<Rectangle> GetOverlayRecangles(List<Rectangle> rectangles)
    {
        List<Rectangle> overLayRecangleList = new List<Rectangle>();
        for (int i = 0; i < rectangles.Count; i++) {
            for (int j = 0; j < rectangles.Count; j++) {
                if (i <= j)  //减少重复计算
                { 
                    continue;
                }
                if(CheckOverlay(rectangles[i], rectangles[j])) {
                    if (!overLayRecangleList.Contains(rectangles[i])) {
                        overLayRecangleList.Add(rectangles[i]);
                    }

                    if (!overLayRecangleList.Contains(rectangles[j]))
                    {
                        overLayRecangleList.Add(rectangles[j]);
                    }
                }
            }
        }
        return overLayRecangleList;
    }

    /// <summary>
    /// 判断两个矩形是否重叠
    /// </summary>
    /// <returns></returns>
    private bool CheckOverlay(Rectangle rect1, Rectangle rect2) {
        Vector2 p1 = rect1.GetLeftTopPosition();
        Vector3 p2 = rect1.GetRightDownPosition();
        Vector3 p3 = rect2.GetLeftTopPosition();
        Vector3 p4 = rect2.GetRightDownPosition();
        if (p1.y <= p4.y || p3.y <= p2.y || p1.x >= p4.x || p2.x <= p3.x)
        {
            return false;
        }
        else {
            return true;
        }
    }
}

public class Rectangle {
    private Vector2 _widthAndLength;
    private Vector2 _centerPosition;
    private int _id;
    public Rectangle(Vector2 widthAndLength, Vector2 centerPosition, int id) {
        _widthAndLength = widthAndLength;
        _centerPosition = centerPosition;
        _id = id;
    }

    public int GetId() {
        return _id;
    }
    public Vector2 GetWidthAndLength() {
        return _widthAndLength;
    }

    public Vector2 GetCenterPosition(){
        return _centerPosition;
    }

    public Vector2 GetLeftTopPosition() {
        return new Vector2(_centerPosition.x - _widthAndLength.x / 2, _centerPosition.y + _widthAndLength.y / 2);
    }

    public Vector2 GetRightDownPosition() {
        return new Vector2(_centerPosition.x + _widthAndLength.x / 2, _centerPosition.y - _widthAndLength.y / 2);
    }
}
