using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y)
    {
        isWall = _isWall;
        x = _x;
        y = _y;
    }

    public bool isWall;
    public Node ParentNode;
    public int[] idx = new int[2];
    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}

public class monsterControl : MonoBehaviour
{

    Rigidbody2D monsterRigidbody;
    public GameObject trg;
    public float speed = 5f;
    public float detectDistance;
    public float distToTarget;
    public float onceDetectDistance;
    bool onceDetect;

    public Vector2Int targetPos;
    public List<Node> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;
    Vector2 moveVec;

    int tileLength = 35;
    int tileSize = 2;
    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;


    int calcNodeIDX(float trg, int isXY)
    {//isXY = 0 -> x, isXY = 1 -> y

        int ret;
        float nowPos = 0;

        if (isXY == 0)
            nowPos = transform.position.x;
        else if (isXY == 1)
            nowPos = transform.position.y;

        //transform.position.x - i *tileSize + (tileLength / 2)*tileSize
        ret = (tileLength / 2)+(int)(nowPos - trg) / tileSize;

        return ret;
    }


    public void pathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        NodeArray = new Node[tileLength, tileLength];

        for (int i = 0; i < tileLength; i++)
        {
            for (int j = 0; j < tileLength; j++)
            {
                bool isWall = false;
                float size = tileSize/5*4;//벽이 있는곳의 기준 크기 (확인하는 곳에서 반지름이 size인 원으로 확인)
                int x = (int)transform.position.x - i * tileSize + (tileLength / 2) * tileSize;
                int y = (int)transform.position.y - j * tileSize + (tileLength / 2) * tileSize;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(x,y), size))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
                        isWall = true;
                NodeArray[i, j] = new Node(isWall, x, y);
                NodeArray[i, j].idx[0] = i;
                NodeArray[i, j].idx[1] = j;
            }
        }
        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[tileLength/2, tileLength/2];
        TargetNode = NodeArray[calcNodeIDX(targetPos.x,0), calcNodeIDX(targetPos.y, 1)];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                    CurNode = OpenList[i];
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // 원하는 타겟노드를 찾았을때 종료
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                
                return;
            }

            int curX = CurNode.idx[0];
            int curY = CurNode.idx[1];
            if (allowDiagonal)
            {
                OpenListAdd(curX + 1, curY + 1);
                OpenListAdd(curX - 1, curY + 1);
                OpenListAdd(curX - 1, curY - 1);
                OpenListAdd(curX + 1, curY - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(curX, curY + 1);
            OpenListAdd(curX + 1, curY);
            OpenListAdd(curX, curY - 1);
            OpenListAdd(curX - 1, curY);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        //확인하는 곳이 혹시나 타일의 범위를 넘어갈 경우 넘어가도록 함
        if (checkX >= tileLength || checkX < 0 || checkY >= tileLength || checkY < 0)
            return;
        // 벽이 아니면서, 닫힌리스트에 없다면
        if (!NodeArray[checkX, checkY].isWall
            && !ClosedList.Contains(NodeArray[checkX, checkY]))
        {
            
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal)
                if (NodeArray[checkX, checkY].isWall && NodeArray[checkX, checkY].isWall)
                    return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner)
                if (NodeArray[checkX, checkY].isWall || NodeArray[checkX, checkY].isWall)
                    return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX, checkY];
            int MoveCost = CurNode.G + (CurNode.x - NodeArray[checkX,checkY].x == 0 || CurNode.y - NodeArray[checkX, checkY].y == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;
                NeighborNode.idx[0] = checkX;
                NeighborNode.idx[1] = checkY;
                OpenList.Add(NeighborNode);
            }
        }
    }

    void moveToTrg()
    {
        pathFinding();
        if (FinalNodeList.Count > 1)
            moveVec = new Vector2(FinalNodeList[1].x, FinalNodeList[1].y);
        transform.position = Vector2.MoveTowards(transform.position, moveVec, speed * Time.deltaTime);
        onceDetect = true;
    }
    //경로 표시
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), detectDistance);
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), onceDetectDistance);
        Gizmos.color = Color.red;
        for (int i = 0; i < tileLength; i++)
            for (int j = 0; j < tileLength; j++)
                Gizmos.DrawWireCube(new Vector2(transform.position.x - i * tileSize + (tileLength / 2) * tileSize, transform.position.y - j * tileSize + (tileLength / 2) * tileSize), new Vector2(tileSize,tileSize));
        Gizmos.color = Color.blue;
        
        if (FinalNodeList.Count != 0)
            for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
    }
    */
   
    void Start()
    {
        trg = GameObject.FindWithTag("Player");
        detectDistance = (int)(tileLength*tileSize/2/4)*3;
        onceDetectDistance = (int)tileLength * tileSize / 2;
        onceDetect = false;
        allowDiagonal = true;
        monsterRigidbody = GetComponent<Rigidbody2D>();
        moveVec = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {

        targetPos = new Vector2Int((int)trg.transform.position.x, (int)trg.transform.position.y);
        distToTarget = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(trg.transform.position.x, trg.transform.position.y));

        if (distToTarget > onceDetectDistance)
            onceDetect = false;

        if (((distToTarget < detectDistance) || (onceDetect && onceDetectDistance > distToTarget))
            && distToTarget > 5.1f)
        {
            moveToTrg();
        }
    }
}