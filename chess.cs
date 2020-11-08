// Just started learning C# in school
// this chess game is made pretty fast but works kinda
// TODO: if king or space to traverse to castle is under attack make castling not possible
// TODO: make array that indicates which part of board is under attack
// TODO: make pawn queens at the end or let player select
// TODO: get en passant moves
// TODO: if invaid selection throw error before letting player select where to move
// TODO: make computer do random moves to play against
// TODO: maybe create a timer mode
// TODO: if king is killed stop game & congrat winner
// TODO: make draw if 3 fold repitiion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Skak
{
    // board[64] is 1 if white rook on 56 moved
    // board[65] is 1 if white king moved
    // board[66] is 1 if white rook on 63 moved
    // board[67] is 1 if black rook on 0 moved
    // board[68] is 1 if black king moved
    // board[69] is 1 if black rook on 7 moved
    // board[70] is to put the dead pieces into
    ///////////////////////////////
    // alias for unicode char
    static readonly char[] black = {'\u2654', '\u2655', '\u2656', '\u2657', '\u2658', '\u2659'};
    static readonly char[] white = {'\u265A', '\u265B', '\u265C', '\u265D', '\u265E', '\u265F'};

    static readonly string columnLetters = "hgfedcba";
    static readonly string columnLettersBlack = "abcdefgh";

    public const char bking = '\u2654';
    public const char bqueen = '\u2655';
    public const char brook = '\u2656';
    public const char bbishop = '\u2657';
    public const char bknight = '\u2658';
    public const char bpawn = '\u2659';
    public const char wking = '\u265A';
    public const char wqueen = '\u265B';
    public const char wrook = '\u265C';
    public const char wbishop = '\u265D';
    public const char wknight = '\u265E';
    public const char wpawn = '\u265F';

    // arrays to tell what is a row and column
    public static int[] hColumn = new int[] {0,1,2,3,4,5,6,7};
    public static int[] gColumn = new int[] {8,9,10,11,12,13,14,15};
    public static int[] fColumn = new int[] {16,17,18,19,20,21,22,23};
    public static int[] eColumn = new int[] {24,25,26,27,28,29,30,31};
    public static int[] dColumn = new int[] {32,33,34,35,36,37,38,39};
    public static int[] cColumn = new int[] {40,41,42,43,44,45,46,47};
    public static int[] bColumn = new int[] {48,49,50,51,52,53,54,55};
    public static int[] aColumn = new int[] {56,57,58,59,60,61,62,63};

    public static int[] oneRow = new int[] {0,8,16,24,32,40,48,56};
    public static int[] twoRow = new int[] {1,9,17,25,33,41,49,57};
    public static int[] threeRow = new int[] {2,10,18,26,34,42,50,58};
    public static int[] fourRow = new int[] {3,11,19,27,35,43,51,59};
    public static int[] fiveRow = new int[] {4,12,20,28,36,44,52,60};
    public static int[] sixRow = new int[] {5,13,21,29,37,45,53,61};
    public static int[] sevenRow = new int[] {6,14,22,30,38,46,54,62};
    public static int[] eightRow = new int[] {7,15,23,31,39,47,55,63};

    // arrays to tell the diagonals that can be traversed
    // 7 diff
    public static int[] g2diag = new int[] {8,1};
    public static int[] f3diag = new int[] {16,9,2};
    public static int[] e4diag = new int[] {24,17,10,3};
    public static int[] d5diag = new int[] {32,25,18,11,4};
    public static int[] c6diag = new int[] {40,33,26,19,12,5};
    public static int[] b7diag = new int[] {48,41,34,27,20,13,6};
    public static int[] a8diag = new int[] {56,49,42,35,28,21,14,7};

    public static int[] twoGdiag = new int[] {57,50,43,36,29,22,15};
    public static int[] threeFdiag = new int[] {58,51,44,37,30,23};
    public static int[] fourEdiag = new int[] {59,52,45,38,31};
    public static int[] fiveDdiag = new int[] {60,53,46,39};
    public static int[] sixCdiag = new int[] {61,54,47};
    public static int[] sevenBdiag = new int[] {62,55};

    // 9 diff
    public static int[] b2diag = new int[] {48,57};
    public static int[] c3diag = new int[] {40,49,58};
    public static int[] d4diag = new int[] {32,41,50,59};
    public static int[] e5diag = new int[] {24,33,42,51,60};
    public static int[] f6diag = new int[] {16,25,34,43,52,61};
    public static int[] g7diag = new int[] {8,17,26,35,44,53,62};
    public static int[] h8diag = new int[] {0,9,18,27,36,45,54,63};

    public static int[] twoBdiag = new int[] {1,10,19,28,37,46,55};
    public static int[] threeCdiag = new int[] {2,11,20,29,38,47};
    public static int[] fourDdiag = new int[] {3,12,21,30,39};
    public static int[] fiveEdiag = new int[] {4,13,22,31};
    public static int[] sixFdiag = new int[] {5,14,23};
    public static int[] sevenGdiag = new int[] {6,15};



    static void Main (){
        // Initialize array for board

        string player = "white";
        char[] board = new char[71];
        char[] checkboard = new char[71];
        char[] wkills = new char[16];
        char[] bkills = new char[16];
        int wkillsCount = 0;
        int bkillsCount = 0;
        bool check;
        string stringMove2;
        string stringPiece;
        string stringPieceBlack;
        int selIndexMove2;
        int selIndexPiece;
        int selIndexPieceBlack;
        ConsoleKeyInfo piecerow;
        ConsoleKeyInfo piececolumn;
        ConsoleKeyInfo move2row;
        ConsoleKeyInfo move2column;



        // Set initial board.
        board[56] = wrook;
        board[63] = wrook;
        board[57] = wknight;
        board[62] = wknight;
        board[58] = wbishop;
        board[61] = wbishop;
        board[59] = wqueen;
        board[60] = wking;

        // for testing castling
        // white
        // board[57] = ' ';
        // board[62] = ' ';
        // board[58] = ' ';
        // board[61] = ' ';
        // board[59] = ' ';


        board[0] = brook;
        board[7] = brook;
        board[1] = bknight;
        board[6] = bknight;
        board[2] = bbishop;
        board[5] = bbishop;
        board[3] = bqueen;
        board[4] = bking;

        // for testing castling
        //black
        // board[1] = ' ';
        // board[2] = ' ';
        // board[3] = ' ';
        // board[5] = ' ';
        // board[6] = ' ';

        // insert 0 in the check fields for moved pieces
        // and index 70 for sending dead pieces to list
        board = recInsert(board, 64, 70, '0');
        // insert pawns and empty space
        board = recInsert(board, 8, 15, bpawn);
        board = recInsert(board, 48, 55, wpawn);
        board = recInsert(board, 16, 47, ' ');

        while (true){
            // copy board into checkboard to check after move if it was valid
            // used to check if the player turn should change
            string strcheckboard;
            string strboard = new string(board);
            strcheckboard = String.Copy(strboard);
            checkboard = strcheckboard.ToCharArray();


            // Get piece player want to move
            Console.Clear();
            Console.Write($"Player {player} select the piece you want to move\nFirst press the column(letter)...\n\n");
            printPiece(board, 99, "column", wkills, bkills, player, 99);
            piececolumn = Console.ReadKey(true);
            Console.Clear();
            Console.Write($"You chose the column: {piececolumn.KeyChar}\nNow chose the row(number)...\n\n");
            printPiece(board, 99, "row", wkills, bkills, player, 99);
            piecerow = Console.ReadKey(true);
            Console.Clear();
            // convert thw two input chars to sting
            stringPiece = charsToString(piececolumn.KeyChar, piecerow.KeyChar);
            // send string to method to find the array index of selection
            selIndexPiece = selection2Index(stringPiece);

            // same for black selection (just to show sel not used for moves)
            stringPieceBlack = charsToString(piececolumn.KeyChar, piecerow.KeyChar);
            selIndexPieceBlack = selection2IndexBlack(stringPieceBlack);

            // Get where player want to move the selected piece
            Console.Write("Select the field you want to move to\nFirst press the column(letter)...\n\n");
            printPiece(board, selIndexPiece, "column", wkills, bkills, player, selIndexPieceBlack);
            move2column = Console.ReadKey(true);
            Console.Clear();
            Console.Write($"You chose the column: {move2column.KeyChar}\nNow chose the row(number)...\n\n");
            printPiece(board, selIndexPiece, "row", wkills, bkills, player, selIndexPieceBlack);
            move2row = Console.ReadKey(true);

            // convert the two input chars to string
            stringMove2 = charsToString(move2column.KeyChar, move2row.KeyChar);
            // send string to function that finds the field index
            selIndexMove2 = selection2Index(stringMove2);


            if (selIndexMove2 < 64 && selIndexPiece < 64) {
                board = movePiece(board, selIndexMove2, selIndexPiece, player);
            }
            // Get bool depen on if the board has changed(player made legit mode) or not (player did not make a legit move so dont change player)
            check = board.SequenceEqual(checkboard);
            if (!(check)){
                if(player == "white" && board[70] != '0'){
                    wkills[wkillsCount] = board[70];
                    wkillsCount++;
                    board[70] = '0';
                }
                if(player == "black" && board[70] != '0'){
                    bkills[bkillsCount] = board[70];
                    bkillsCount++;
                    board[70] = '0';
                }
                // Change current player
                player = changePlayer(player);
            }
            else{
                Console.WriteLine("Your move was invalid! Press a key to continue...");
                Console.Read();
            }

        }
    }
    // static void timer();
    // static void checkwin();
    // static void check();

    static string changePlayer(string player)
    {
        if ( player == "white" ){
            player = "black";
        }
        else{
            player = "white";
        }
        return player;
    }

    static char[] movePiece(char[] board, int indexMove2, int indexPiece, string player){
        // bool clearPath;
        char charPiece = board[indexPiece];
        if (player == "white"){
            switch(charPiece){
                case wpawn:
                    if(indexPiece-8 == indexMove2 && board[indexMove2] == ' '){
                        board[indexPiece] = ' ';
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece >= 48 && indexPiece-16 == indexMove2 && board[indexMove2] == ' '){
                        board[indexPiece] = ' ';
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece-9 == indexMove2 && (Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        board[70] = board[indexMove2];
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece-7 == indexMove2 && (Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        board[70] = board[indexMove2];
                        board[indexMove2] = charPiece;
                    }
                    break;
                case wrook:
                    board = lineMove(board, indexPiece, indexMove2, 8, oneRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, twoRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, threeRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, fourRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, fiveRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, sixRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, sevenRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, eightRow, white);

                    board = lineMove(board, indexPiece, indexMove2, 1, aColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, bColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, cColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, dColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, eColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, fColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, gColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, hColumn, white);

                    break;
                case wking:
                    if(indexPiece-8 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy, move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[65] = '1'; //flag king moved
                    }
                    else if(indexPiece-9 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[65] = '1';//flag king moved
                    }
                    else if(indexPiece-7 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[65] = '1';//flag king moved
                    }
                    else if(indexPiece-1 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[65] = '1';//flag king moved
                    }
                    else if(indexPiece+8 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[65] = '1';//flag king moved
                    }
                    else if(indexPiece+9 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[65] = '1';//flag king moved
                    }
                    else if(indexPiece+7 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[65] = '1';//flag king moved
                    }
                    else if(indexPiece+1 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[65] = '1';//flag king moved
                    }
                    // castling to the left(towards index 56 rook)
                    else if(indexPiece-4 == indexMove2 && board[64] == '0' && board[65] == '0' && board[57] == ' ' && board[58] == ' ' && board[59] == ' '){
                        board[indexPiece] = ' ';
                        board[56] = ' ';
                        board[58] = wking;
                        board[59] = wrook;
                        board[65] = '1';//flag king moved
                    }
                    // castling to the right(toawrds index 63 rook)
                    else if(indexPiece+3 == indexMove2 && board[66] == '0' && board[65] == '0' && board[61] == ' ' && board[62] == ' '){
                        board[indexPiece] = ' ';
                        board[63] = ' ';
                        board[62] = wking;
                        board[61] = wrook;
                        board[65] = '1';//flag king moved
                    }
                    break;
                case wknight:
                    if(indexPiece-17 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece-15 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece-10 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece-6 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+17 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+15 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+10 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+6 == indexMove2 && !(Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    break;
                case wbishop:
                    board = lineMove(board, indexPiece, indexMove2, 7, g2diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, f3diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, e4diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, d5diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, c6diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, b7diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, a8diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, twoGdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, threeFdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, fourEdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, fiveDdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, sixCdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, sevenBdiag, white);

                    board = lineMove(board, indexPiece, indexMove2, 9, b2diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, c3diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, d4diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, e5diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, f6diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, g7diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, h8diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, twoBdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, threeCdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, fourDdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, fiveEdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, sixFdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, sevenGdiag, white);
                    break;
                case wqueen:

                    board = lineMove(board, indexPiece, indexMove2, 8, oneRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, twoRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, threeRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, fourRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, fiveRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, sixRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, sevenRow, white);
                    board = lineMove(board, indexPiece, indexMove2, 8, eightRow, white);

                    board = lineMove(board, indexPiece, indexMove2, 1, aColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, bColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, cColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, dColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, eColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, fColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, gColumn, white);
                    board = lineMove(board, indexPiece, indexMove2, 1, hColumn, white);

                    board = lineMove(board, indexPiece, indexMove2, 7, g2diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, f3diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, e4diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, d5diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, c6diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, b7diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, a8diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, twoGdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, threeFdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, fourEdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, fiveDdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, sixCdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 7, sevenBdiag, white);

                    board = lineMove(board, indexPiece, indexMove2, 9, b2diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, c3diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, d4diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, e5diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, f6diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, g7diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, h8diag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, twoBdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, threeCdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, fourDdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, fiveEdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, sixFdiag, white);
                    board = lineMove(board, indexPiece, indexMove2, 9, sevenGdiag, white);
                    break;
            }
            return board;
        }
        if (player == "black"){
            switch(charPiece){
                case bpawn:
                    if(indexPiece+8 == indexMove2 && board[indexMove2] == ' '){
                        board[indexPiece] = ' ';
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece <= 15 && indexPiece+16 == indexMove2 && board[indexMove2] == ' '){
                        board[indexPiece] = ' ';
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+9 == indexMove2 && (Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        board[70] = board[indexMove2];
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+7 == indexMove2 && (Array.Exists(white, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        board[70] = board[indexMove2];
                        board[indexMove2] = charPiece;
                    }
                    break;
                case bking:
                    if(indexPiece-8 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[68] = '1'; //flag king moved
                    }
                    else if(indexPiece-9 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[68] = '1'; //flag king moved
                    }
                    else if(indexPiece-7 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[68] = '1'; //flag king moved
                    }
                    else if(indexPiece-1 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[68] = '1'; //flag king moved
                    }
                    else if(indexPiece+8 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[68] = '1'; //flag king moved
                    }
                    else if(indexPiece+9 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[68] = '1'; //flag king moved
                    }
                    else if(indexPiece+7 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[68] = '1'; //flag king moved
                    }
                    else if(indexPiece+1 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                        board[68] = '1'; //flag king moved
                    }
                    // castling to the left (black perspective)
                    else if(indexPiece+3 == indexMove2 && board[69] == '0' && board[68] == '0' && board[5] == ' ' && board[6] == ' '){
                        board[indexPiece] = ' ';
                        board[7] = ' ';
                        board[6] = bking;
                        board[5] = brook;
                        board[68] = '1'; //flag king moved
                    }
                    // castling to the right (black perspective)
                    else if(indexPiece-4 == indexMove2 && board[67] == '0' && board[68] == '0' && board[1] == ' ' && board[2] == ' ' && board[3] == ' '){
                        board[indexPiece] = ' ';
                        board[0] = ' ';
                        board[2] = bking;
                        board[3] = brook;
                        board[68] = '1'; //flag king moved
                    }
                    break;
                case bknight:
                    if(indexPiece-17 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece-15 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece-10 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece-6 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+17 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+15 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+10 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    else if(indexPiece+6 == indexMove2 && !(Array.Exists(black, element => element == board[indexMove2]))){
                        board[indexPiece] = ' ';
                        if (board[indexMove2] != ' '){ // if enemy move to kills array
                            board[70] = board[indexMove2];
                        }
                        board[indexMove2] = charPiece;
                    }
                    break;
                case bbishop:
                    board = lineMove(board, indexPiece, indexMove2, 7, g2diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, f3diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, e4diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, d5diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, c6diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, b7diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, a8diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, twoGdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, threeFdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, fourEdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, fiveDdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, sixCdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, sevenBdiag, black);

                    board = lineMove(board, indexPiece, indexMove2, 9, b2diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, c3diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, d4diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, e5diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, f6diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, g7diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, h8diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, twoBdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, threeCdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, fourDdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, fiveEdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, sixFdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, sevenGdiag, black);
                    break;
                case brook:
                    board = lineMove(board, indexPiece, indexMove2, 8, oneRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, twoRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, threeRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, fourRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, fiveRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, sixRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, sevenRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, eightRow, black);

                    board = lineMove(board, indexPiece, indexMove2, 1, aColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, bColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, cColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, dColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, eColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, fColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, gColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, hColumn, black);

                    break;
                case bqueen:

                    board = lineMove(board, indexPiece, indexMove2, 8, oneRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, twoRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, threeRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, fourRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, fiveRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, sixRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, sevenRow, black);
                    board = lineMove(board, indexPiece, indexMove2, 8, eightRow, black);

                    board = lineMove(board, indexPiece, indexMove2, 1, aColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, bColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, cColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, dColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, eColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, fColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, gColumn, black);
                    board = lineMove(board, indexPiece, indexMove2, 1, hColumn, black);

                    board = lineMove(board, indexPiece, indexMove2, 7, g2diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, f3diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, e4diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, d5diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, c6diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, b7diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, a8diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, twoGdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, threeFdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, fourEdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, fiveDdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, sixCdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 7, sevenBdiag, black);

                    board = lineMove(board, indexPiece, indexMove2, 9, b2diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, c3diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, d4diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, e5diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, f6diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, g7diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, h8diag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, twoBdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, threeCdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, fourDdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, fiveEdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, sixFdiag, black);
                    board = lineMove(board, indexPiece, indexMove2, 9, sevenGdiag, black);
                    break;
            }
            return board;
        }
        return board;
    }

    static string charsToString(char column, char row){
        char[] chars = {column, row};
        string s = new string(chars);
        return s;
    }

    static char[] recInsert(char[] board, int min, int max, char insert){
        for (int i = min; i <= max; ++i){
            board[i] = insert;
        }
        return board;
    }

    static void printPiece(char[] board, int selIndexPiece, string selrowOrcolumn, char[] wkills, char[] bkills, string player, int selIndexPieceBlack){
        if(player == "white"){
            int y = 8;
            int x = 0;
            foreach (char c in columnLetters){
                if(selrowOrcolumn == "column"){
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"({c}) ");
                    Console.ResetColor();
                }
                else{
                    Console.Write($"({c}) ");
                }
                while(x < y){
                    if (x == selIndexPiece){
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"[{board[x]}] ");
                        Console.ResetColor();
                    }
                    else{
                        Console.Write($"[{board[x]}] ");
                    }
                    if ((new []{7, 15, 23, 31, 39, 47, 55, 63}).Contains(x)){
                        Console.WriteLine();
                    }
                    x++;
                }
                y += 8;
            }
            if(selrowOrcolumn == "row"){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"    (1) (2) (3) (4) (5) (6) (7) (8)\n\n");
                Console.ResetColor();
            }
            else{
                Console.Write($"    (1) (2) (3) (4) (5) (6) (7) (8)\n\n");
            }
            Console.Write($"Player white kills: ");
            Console.WriteLine(wkills);
            Console.Write($"Player black kills: ");
            Console.WriteLine(bkills);

            // testing info
            // Console.WriteLine("white rook flag");
            // Console.WriteLine(board[64]);
            // Console.WriteLine("white king flag");
            // Console.WriteLine(board[65]);
            // Console.WriteLine("white rook flag");
            // Console.WriteLine(board[66]);
            // Console.WriteLine("black rook flag");
            // Console.WriteLine(board[67]);
            // Console.WriteLine("black king flag");
            // Console.WriteLine(board[68]);
            // Console.WriteLine("black rook flag");
            // Console.WriteLine(board[69]);
        }
        if(player == "black"){
            char[] blackboard = new char[71];
            Array.Reverse(board, 0, 64);
            string strboardrev = new string(board);
            string strblackboard;
            strblackboard = String.Copy(strboardrev);
            blackboard = strblackboard.ToCharArray();
            Array.Reverse(board, 0, 64);
            int x = 0;
            int y = 8;
            foreach (char c in columnLettersBlack){
                if(selrowOrcolumn == "column"){
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"({c}) ");
                    Console.ResetColor();
                }
                else{
                    Console.Write($"({c}) ");
                }
                while(x < y){
                    if (x == selIndexPieceBlack){
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"[{blackboard[x]}] ");
                        Console.ResetColor();
                    }
                    else{
                        Console.Write($"[{blackboard[x]}] ");
                    }
                    if ((new []{7, 15, 23, 31, 39, 47, 55, 63}).Contains(x)){
                        Console.WriteLine();
                    }
                    x++;
                }
                y += 8;
            }
            if(selrowOrcolumn == "row"){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"    (8) (7) (6) (5) (4) (3) (2) (1)\n\n");
                Console.ResetColor();
            }
            else{
                Console.Write($"    (8) (7) (6) (5) (4) (3) (2) (1)\n\n");
            }
            Console.Write($"Player white taken pieces: ");
            Console.WriteLine(wkills);
            Console.Write($"Player black taken pieces: ");
            Console.WriteLine(bkills);

            // testing info
            Console.WriteLine("white rook flag");
            Console.WriteLine(board[64]);
            Console.WriteLine("white king flag");
            Console.WriteLine(board[65]);
            Console.WriteLine("white rook flag");
            Console.WriteLine(board[66]);
            Console.WriteLine("black rook flag");
            Console.WriteLine(board[67]);
            Console.WriteLine("black king flag");
            Console.WriteLine(board[68]);
            Console.WriteLine("black rook flag");
            Console.WriteLine(board[69]);
        }
    }

    static char[] lineMove(char[] board, int indexPiece, int indexMove2, int addNumber, int[] line, char[] color){
        bool clearPath = true;
        char charPieceMove = board[indexPiece];
        if(Array.Exists(line, element => element == indexPiece) && Array.Exists(line, element => element == indexMove2)){
            int max = Math.Max(indexPiece, indexMove2);
            int min = Math.Min(indexPiece, indexMove2);
            while(min < max){
                min = min + addNumber;
                if(board[min] != ' '){
                    if(max == min){
                        break;
                    }
                    clearPath = false;
                    break;
                }
            }
            if(!(Array.Exists(color, element => element == board[indexMove2])) && (clearPath == true)){ // if not white at indexmove2 and clearPath is true complete move
                board[indexPiece] = ' ';
                if (board[indexMove2] != ' '){ // if enemy move to kills array
                    board[70] = board[indexMove2];
                }
                board[indexMove2] = charPieceMove;
                if((indexPiece == 56) && (charPieceMove == wrook)){ // put flag if moving from wrook start index. check for wrook kinda not needed?
                    board[64] = '1';
                }
                if((indexPiece == 63) && (charPieceMove == wrook)){ // put flag if moving from wrook start index. check for wrook kinda not needed?
                    board[66] = '1';
                }
                if((indexPiece == 0) && (charPieceMove == brook)){ // put flag if moving from wrook start index. check for wrook kinda not needed?
                    board[67] = '1';
                }
                if((indexPiece == 7) && (charPieceMove == brook)){ // put flag if moving from wrook start index. check for wrook kinda not needed?
                    board[69] = '1';
                }
            }
        }
        return board;
    }

    static int selection2Index(string s){
        int returnValue = 0;
        foreach(char c in columnLetters){
            for(int i = 1; i <= 8; i++){
                string concatString = c + Convert.ToString(i);
                if(concatString == s){
                    return returnValue;
                }
                returnValue++;
            }
        }
        return 99;
    }

    static int selection2IndexBlack(string s){
        int returnValue = 63;
        foreach(char c in columnLetters){
            for(int i = 1; i <= 8; i++){
                string concatString = c + Convert.ToString(i);
                if(concatString == s){
                    return returnValue;
                }
                returnValue--;
            }
        }
        return 99;
    }
}

