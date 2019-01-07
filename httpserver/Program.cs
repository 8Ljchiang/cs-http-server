using System;
using System.Linq;
using System.Collections.Generic;

namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {
            Func<Request, Response, string, Response> getIndexHtmlController = (Request req, Response res, string contextData) =>
            {
                string pageHTML = $"<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n\t<meta charset=\"UTF-8\">\n\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n\t<meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\">\n\t\n\t<title>Tic Tac Toe</title>\n\t<script crossorigin src=\"https://unpkg.com/react@16/umd/react.development.js\"></script>\n\t<script crossorigin src=\"https://unpkg.com/react-dom@16/umd/react-dom.development.js\"></script>\n\t<link rel=\"stylesheet\" href=\"http://{contextData}/index.css\">\n</head>\n<body>\n\t<div id=\"root\"></div>\n\t<script src=\"http://{contextData}/index.js\" ></script>\t\n</body>\n</html>";
                res.AddHeader("Content-Type", "text/html");
                res.Body = pageHTML;
                return res;
            };

            Func<Request, Response, string, Response> getIndexCssController = (Request req, Response res, string contextData) =>
            {
                string pageCSS = "*{box-sizing:border-box;}.list-item{padding:10px;border:1px solid gray}.page{align-items:center;background:rgba(38,43,51,1);display:flex;height:100vh;justify-content:center;padding:10px;width:100%}.table{align-items:center;background:rgba(47,52,63,1);border-radius:4px;box-shadow:0 0 8px 5px rgba(22,25,37,1);display:flex;width:640px;flex-flow:column}.table__header{align-items:center;border-radius:4px 4px 0 0;background:steelblue;display:flex;height:40px;justify-content:space-between;padding:8px;width:100%}.table__body{border-radius:0 0 4px 4px;height:640px;display:flex;flex-flow:column;flex:1;width:100%}.column{align-items:center;border:1 solid white;display:flex;flex:1;height:100%;margin:5px;width:100%}.column__box{align-items:center;border:1px dashed white;border-radius:4px;display:flex;justify-content:center;height:200px;margin:0 5px 0 5px;width:200px}.column__box--highlight{border:2px solid rgb(105,194,45)}.box__title{align-content:center;color:white;display:flex;justify-content:center}p{font-family:'Courier New',Courier,monospace}.header__title{color:black}.message__queue{display:flex;flex-flow:column;height:100%;left:0;padding:10px;position:fixed;top:0}.message__container{align-items:center;border-radius:4px;box-shadow:0 0 8px 5px rgba(22,25,37,.63);display:flex;height:50px;margin-bottom:10px;justify-content:center;padding:4px;width:200px}.message__container--success{background:yellowgreen}.message__container--warning{background:yellow}.message__container--error{background:palevioletred}.message__container--default{background:lightgray}.message__container__title{color:black}.animated-slow-2{-webkit-animation-duration:3.2s;animation-duration:3.8s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease}.animated-slow-1{-webkit-animation-duration:2.8s;animation-duration:3.3s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease}.animated-2{-webkit-animation-duration:2s;animation-duration:2.5s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease}.animated{-webkit-animation-duration:2s;animation-duration:2s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease}.animated-med{-webkit-animation-duration:2s;animation-duration:1.2s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease-in}.animated-fast{-webkit-animation-duration:2s;animation-duration:0.8s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease-in}@-webkit-keyframes fadeInDown{0%{opacity:0;-webkit-transform:translateY(-50px)}100%{opacity:1;-webkit-transform:translateY(0)}}@keyframes fadeInDown-shadowless{0%{opacity:0;transform:translateY(-32px)}100%{opacity:1;transform:translateY(0)}}@keyframes fadeInDown{0%{opacity:0;box-shadow:4px 0 14px 14px rgba(22,25,37,.6);transform:translateY(-44px)}100%{opacity:1;box-shadow:0 0 8px 5px rgba(22,25,37,.8);transform:translateY(0)}}.fadeInDown{-webkit-animation-name:fadeInDown;animation-name:fadeInDown}.fadeInDown-shadowless{-webkit-animation-name:fadeInDown-shadowless;animation-name:fadeInDown-shadowless}";
                res.Body = pageCSS;
                res.AddHeader("Content-Type", "text/css");
                return res;
            };

            Func<Request, Response, string, Response> getIndexJsController = (Request req, Response res, string contextData) =>
            {
                string pageJS = "const validPositions=[1,2,3,4,5,6,7,8,9];const winningPatterns=['1,2,3','4,5,6','7,8,9','1,4,7','2,5,8','3,6,9','1,5,9','3,5,7',];const gameTitle='Tic Tac Toe by Jonathan Chiang';const playerMarker='X';const e=React.createElement;function getMatchingPattern(moves,marker){for(let pattern of winningPatterns){const patternArr=pattern.split(',').map(p=>parseInt(p));const cache=[];for(let move of moves){if(patternArr.includes(move.position)){cache.push(move.position)}}\nif(cache.length===patternArr.length){return patternArr}}\nreturn[]}\nconst TableHeader=props=>{const message={status:'default',text:'This game was made with HTML, CSS, and JS'}\nconst animatedString=' animated-med fadeInDown-shadowless';return e('div',{className:'table__header',onClick:()=>props.actions.addMessage(message)},[e('p',{key:'header-title',className:'header__title'+animatedString},gameTitle),e('div',{key:'header-reset-button',className:'header__reset-button',onClick:()=>{props.actions.resetMoves()}},e('p',{className:animatedString.trim()},'Restart Game'))])};const TableBody=props=>{const boxes=[];for(let i=1;i<=9;i++){const moveArr=props.moves.filter((move)=>{return move.position==i});const value=(moveArr.length>0&&moveArr[0])?moveArr[0].value:' ';const highlight=props.winningPattern.includes(i);boxes.push(e(ColumnBox,{key:i,position:i,value,highlight,actions:props.actions}))}\nconst columnClassName='column';const columns=[];columns.push(e('div',{key:'left-column',className:columnClassName},boxes.slice(0,3)));columns.push(e('div',{key:'mid-column',className:columnClassName},boxes.slice(3,6)));columns.push(e('div',{key:'right-column',className:columnClassName},boxes.slice(6,9)));return e('div',{className:'table__body'},columns)}\nconst Column=props=>{return e('div',{className:'column'})}\nconst ColumnBox=props=>{let classNameString=props.highlight?'column__box column__box--highlight':'column__box';let animationString='';if(props.position>=1&&props.position<=3){animationString='animated-2 fadeInDown'}else if(props.position>=4&&props.position<=6){animationString='animated-slow-1 fadeInDown'}else{animationString='animated-slow-2 fadeInDown'}\nclassNameString=classNameString+\" \"+animationString;if(props.value===' '){const message={status:'success',text:`Position ${props.position}: Set value to ${playerMarker}.`}\nreturn e('div',{className:classNameString,onClick:()=>{props.actions.addPosition(props.position);props.actions.addMessage(message)}},e('p',{className:'box__title'},props.value))}else{const message={status:'warning',text:`Position ${props.position}: '${props.value}' is already taken.`}\nreturn e('div',{className:classNameString,onClick:()=>props.actions.addMessage(message)},e('p',{className:'box__title'},props.value))}};class Table extends React.Component{render(){const{actions,moves,winningPattern}=this.props;return e('div',{className:'table animated-med fadeInDown'},[e(TableHeader,{key:'table-header',actions}),e(TableBody,{key:'table-body',moves,actions,winningPattern})])}}\nconst MessageQueue=props=>{const messages=props.messages.map((message,index)=>{return e(Message,{key:index,message:message.text,status:message.status})});return e('div',{className:'message__queue'},messages)}\nconst Message=props=>{const animatedString=' animated-fast fadeInDown';switch(props.status){case 'success':return e('div',{className:'message__container message__container--success'+animatedString},e('p',{className:'message__container__title'},props.message));case 'warning':return e('div',{className:'message__container message__container--warning'+animatedString},e('p',{className:'message__container__title'},props.message));case 'default':return e('div',{className:'message__container message__container--default'+animatedString},e('p',{className:'message__container__title'},props.message));case 'error':return e('div',{className:'message__container message__container--error'+animatedString},e('p',{className:'message__container__title'},props.message));default:return e('div',{className:'message__container message__container--default'+animatedString},e('p',{className:'message__container__title'},props.message))}}\nclass Page extends React.Component{constructor(){super();this.state={moves:[],messages:[],winningPattern:[],}\nthis.addPosition=this.addPosition.bind(this);this.addMessage=this.addMessage.bind(this);this.clearMessages=this.clearMessages.bind(this);this.clearFirstMessage=this.clearFirstMessage.bind(this);this.resetMoves=this.resetMoves.bind(this)}\naddPosition(position){const takenPositions=this.state.moves.map((move)=>{return move.position});if(validPositions.includes(position)&&!takenPositions.includes(position)){const newMove={position,value:'X'}\nconst newMoves=[...this.state.moves,newMove];const matchingPattern=getMatchingPattern(newMoves.filter((move)=>{return newMove.value===move.value}),newMove.value);this.setState({moves:newMoves,winningPattern:matchingPattern})}}\naddMessage(message){this.setState({messages:[...this.state.messages,message]});setTimeout(this.clearFirstMessage,3000)}\nclearMessages(){this.setState({messages:[]})}\nresetMoves(){this.setState({moves:[],winningPattern:[]})}\nasync clearFirstMessage(){if(this.state.messages.length>1){this.setState({messages:this.state.messages.slice(1)})}else{this.clearMessages()}}\nrender(){const actions={addPosition:this.addPosition,addMessage:this.addMessage,resetMoves:this.resetMoves}\nconst{moves,messages,winningPattern}=this.state;const message={status:'error',text:'Click detected outside of playable area'}\nreturn e('div',{key:'page',className:'page'},[e(MessageQueue,{key:'message-queue',className:'message__queue',messages}),e(Table,{key:'table',actions,moves,winningPattern}),])}};ReactDOM.render(e(Page),document.getElementById('root'))";
                res.Body = pageJS;
                res.AddHeader("Content-Type", "text/javascript");
                return res;
            };

            Func<Request, Response, string, Response> testController = (Request req, Response res, string contextData) =>
            {
                res.Body = "{ response: success }";
                res.AddHeader("Content-Type", "application/json");
                return res;
            };

            Func<Request, Response, string, Response> randomMoveController = (Request req, Response res, string contextData) =>
            {
                string[] moves = req.Body.Split(",");

                List<int> emptyPositions = new List<int>();

                for (int i = 1; i <= 9; i++)
                {
                    if (!moves.Contains(i.ToString()))
                    {
                        emptyPositions.Add(i);
                    }
                }

                if (emptyPositions.Count > 0)
                {
                    Random rnd = new Random();

                    int randomIndex = rnd.Next(1, emptyPositions.Count);
                    string position = emptyPositions[randomIndex].ToString();
                    res.Body = "{ \"move\": \"" + position + "\" }";
                }
                else
                {
                    res.Body = "{ \"move\": \"null\" }";
                }

                res.AddHeader("Content-Type", "application/json");
                return res;
            };

            Func<Request, Response, string, Response> getIndex2HtmlController = (Request req, Response res, string contextData) =>
            {
                string version = "2";
                string pageHTML = $"<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n\t<meta charset=\"UTF-8\">\n\t<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n\t<meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\">\n\t\n\t<title>Tic Tac Toe</title>\n\t<script crossorigin src=\"https://unpkg.com/react@16/umd/react.development.js\"></script>\n\t<script crossorigin src=\"https://unpkg.com/react-dom@16/umd/react-dom.development.js\"></script>\n\t<link rel=\"stylesheet\" href=\"http://{contextData}/index{version}.css\">\n</head>\n<body>\n\t<div id=\"root\"></div>\n\t<script src=\"http://{contextData}/index{version}.js\" ></script>\t\n</body>\n</html>";
                res.AddHeader("Content-Type", "text/html");
                res.Body = pageHTML;
                return res;
            };

            Func<Request, Response, string, Response> getIndex2CssController = (Request req, Response res, string contextData) =>
            {
                string pageCSS = "*{box-sizing:border-box;padding:0px;margin:0px;}.list-item{padding:10px;border:1px solid gray}.page{align-items:center;background:rgba(38,43,51,1);display:flex;flex-flow:column;height:100vh;justify-content:center;padding:10px;width:100%}.page__play-button{align-items:center;background:rgb(235,109,147);display:flex;justify-content:center;padding:12px;position:fixed;right:40px;width:120px;top:40px;z-index:2}.clock__text{color:lightgray;font-size:4em}.table{align-items:center;background:rgba(47,52,63,1);border-radius:4px;box-shadow:0 0 8px 5px rgba(22,25,37,1);display:flex;width:640px;flex-flow:column}.table--shadowless{align-items:center;border-radius:4px;display:flex;width:640px;flex-flow:column}.table__header{align-items:center;border-radius:4px 4px 0 0;background:steelblue;display:flex;height:40px;justify-content:space-between;padding:8px;width:100%}.table__body{border-radius:0 0 4px 4px;height:640px;display:flex;flex-flow:column;flex:1;width:100%}.column{align-items:center;display:flex;flex:1;height:100%;margin:5px;width:100%}.column__box{align-items:center;border:1px dashed lightgray;border-radius:4px;display:flex;justify-content:center;height:200px;margin:0 5px 0 5px;width:200px}.column__box--highlight{border:2px solid rgb(105,194,45)}.box__title{align-content:center;color:white;display:flex;justify-content:center}p{font-family:'Courier New',Courier,monospace}.header__title{color:black}.message__queue{display:flex;flex-flow:column;height:100%;left:0;padding:10px;position:fixed;top:0}.message__container{align-items:center;border-radius:4px;box-shadow:0 0 8px 5px rgba(22,25,37,.63);display:flex;height:50px;margin-bottom:10px;justify-content:center;padding:4px;width:200px}.message__container--success{background:yellowgreen}.message__container--warning{background:yellow}.message__container--error{background:palevioletred}.message__container--default{background:lightgray}.message__container__title{color:black}.animated-slow-2{-webkit-animation-duration:3.2s;animation-duration:3.8s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease}.animated-slow-1{-webkit-animation-duration:2.8s;animation-duration:3.3s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease}.animated-2{-webkit-animation-duration:2s;animation-duration:2.5s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease}.animated{-webkit-animation-duration:2s;animation-duration:2s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease}.animated-med{-webkit-animation-duration:2s;animation-duration:1.2s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease-in}.animated-fast{-webkit-animation-duration:2s;animation-duration:0.8s;-webkit-animation-fill-mode:both;animation-fill-mode:both;transition-timing-function:ease-in}@-webkit-keyframes fadeInDown{0%{opacity:0;-webkit-transform:translateY(-50px)}100%{opacity:1;-webkit-transform:translateY(0)}}@keyframes fadeInDown-shadowless{0%{opacity:0;transform:translateY(-44px)}100%{opacity:1;transform:translateY(0)}}@keyframes fadeInDown{0%{opacity:0;box-shadow:4px 0 14px 14px rgba(22,25,37,.6);transform:translateY(-44px) translateX(-16px) scale(1.32)}100%{opacity:1;box-shadow:0 0 8px 5px rgba(22,25,37,.8);transform:translateY(0) translateX(0) scale(1)}}@keyframes scaleIn{0%{transform:scale(.8)}100%{transform:scale(1)}}.scaleIn{-webkit-animation-name:scaleIn;animation-name:scaleIn}.fadeInDown{-webkit-animation-name:fadeInDown;animation-name:fadeInDown}.fadeInDown-shadowless{-webkit-animation-name:fadeInDown-shadowless;animation-name:fadeInDown-shadowless}";
                res.Body = pageCSS;
                res.AddHeader("Content-Type", "text/css");
                return res;
            };

            Response getIndex2JsController(Request req, Response res, string contextData)
            {
                string pageJS = "const e=React.createElement;const validPositions=[1,2,3,4,5,6,7,8,9];const winningPatterns=['1,2,3','4,5,6','7,8,9','1,4,7','2,5,8','3,6,9','1,5,9','3,5,7',];const gameTitle='Tic Tac Toe by Jonathan Chiang';const playerMarker='X';let datetimeRefreshInterval;function getMatchingPattern(moves,marker){for(let pattern of winningPatterns){const patternArr=pattern.split(',').map(p=>parseInt(p));console.log('comparing:',pattern,moves.map(m=>m.position).join(','));const cache=[];for(let move of moves){if(patternArr.includes(move.position)){cache.push(move.position)}}\nconsole.log('cache length: ',cache.length,'patternArr',patternArr.length);if(cache.length===patternArr.length){console.log('Matching pattern found');return patternArr}}\nreturn[]}\nconst TableHeader=props=>{const message={status:'default',text:'This game was made with HTML, CSS, and JS'}\nconst animatedString=' animated-med fadeInDown-shadowless';return e('div',{className:'table__header',onClick:()=>props.actions.addMessage(message)},[e('p',{key:'header-title',className:'header__title'+animatedString},gameTitle),e('div',{key:'header-reset-button',className:'header__reset-button',onClick:()=>{props.actions.resetMoves()}},e('p',{className:animatedString.trim()},'Restart Game'))])};const TableBody=props=>{const boxes=[];for(let i=1;i<=9;i++){const moveArr=props.moves.filter((move)=>{return move.position==i});const value=(moveArr.length>0&&moveArr[0])?moveArr[0].value:' ';const highlight=props.winningPattern.includes(i);boxes.push(e(ColumnBox,{key:i,position:i,value,highlight,actions:props.actions}))}\nconst columnClassName='column';const columns=[];columns.push(e('div',{key:'left-column',className:columnClassName},boxes.slice(0,3)));columns.push(e('div',{key:'mid-column',className:columnClassName},boxes.slice(3,6)));columns.push(e('div',{key:'right-column',className:columnClassName},boxes.slice(6,9)));return e('div',{className:'table__body'},columns)}\nconst Column=props=>{return e('div',{className:'column'})}\nconst ColumnBox=props=>{let classNameString=props.highlight?'column__box column__box--highlight':'column__box';let animationString='';if(props.position>=1&&props.position<=3){animationString='animated-2 fadeInDown'}else if(props.position>=4&&props.position<=6){animationString='animated-2 fadeInDown'}else{animationString='animated-2 fadeInDown'}\nclassNameString=classNameString+\" \"+animationString;if(props.value===' '){const message={status:'success',text:`Position ${props.position}: Set value to ${playerMarker}.`}\nreturn e('div',{className:classNameString,onClick:()=>{props.actions.addPosition(props.position);props.actions.addMessage(message)}},e('p',{className:'box__title'},props.value))}else{const message={status:'warning',text:`Position ${props.position}: '${props.value}' is already taken.`}\nreturn e('div',{className:classNameString,onClick:()=>props.actions.addMessage(message)},e('p',{className:'box__title'},props.value))}};class Table extends React.Component{render(){const{actions,moves,winningPattern}=this.props;return e('div',{className:'table animated-med fadeInDown'},[e(TableHeader,{key:'table-header',actions}),e(TableBody,{key:'table-body',moves,actions,winningPattern})])}}\nconst MessageQueue=props=>{const messages=props.messages.map((message,index)=>{return e(Message,{key:index,message:message.text,status:message.status})});return e('div',{className:'message__queue'},messages)}\nconst Message=props=>{const animatedString=' animated-fast fadeInDown';switch(props.status){case 'success':return e('div',{className:'message__container message__container--success'+animatedString},e('p',{className:'message__container__title'},props.message));case 'warning':return e('div',{className:'message__container message__container--warning'+animatedString},e('p',{className:'message__container__title'},props.message));case 'default':return e('div',{className:'message__container message__container--default'+animatedString},e('p',{className:'message__container__title'},props.message));case 'error':return e('div',{className:'message__container message__container--error'+animatedString},e('p',{className:'message__container__title'},props.message));default:return e('div',{className:'message__container message__container--default'+animatedString},e('p',{className:'message__container__title'},props.message))}}\nclass Page extends React.Component{constructor(){super();this.state={moves:[],messages:[],winningPattern:[],isStarted:!1,currentDateTime:new Date()}\nthis.addPosition=this.addPosition.bind(this);this.addMessage=this.addMessage.bind(this);this.clearMessages=this.clearMessages.bind(this);this.clearFirstMessage=this.clearFirstMessage.bind(this);this.resetMoves=this.resetMoves.bind(this);this.toggleStarted=this.toggleStarted.bind(this)}\ncomponentDidMount(){datetimeRefreshInterval=setInterval(()=>this.updateCurrentTime(),1000)}\ntoggleStarted(){if(!this.state.isStarted){clearInterval(datetimeRefreshInterval)}else{datetimeRefreshInterval=setInterval(()=>this.updateCurrentTime(),1000)}\nthis.setState({isStarted:!this.state.isStarted})}\naddPosition(position){const takenPositions=this.state.moves.map((move)=>{return move.position});if(validPositions.includes(position)&&!takenPositions.includes(position)){const newMove={position,value:'X'}\nconst newMoves=[...this.state.moves,newMove];const matchingPattern=getMatchingPattern(newMoves.filter((move)=>{return newMove.value===move.value}),newMove.value);this.setState({moves:newMoves,winningPattern:matchingPattern})}}\naddMessage(message){this.setState({messages:[...this.state.messages,message]});setTimeout(this.clearFirstMessage,3000)}\nclearMessages(){this.setState({messages:[]})}\nresetMoves(){this.setState({moves:[],winningPattern:[]})}\nasync clearFirstMessage(){if(this.state.messages.length>1){this.setState({messages:this.state.messages.slice(1)})}else{this.clearMessages()}}\nrenderTable(){const actions={addPosition:this.addPosition,addMessage:this.addMessage,resetMoves:this.resetMoves}\nconst{moves,winningPattern}=this.state;if(this.state.isStarted){return e(Table,{key:'table',actions,moves,winningPattern})}\nreturn this.renderTime()}\nrenderMessageQueue(){const{messages}=this.state;return e(MessageQueue,{key:'message-queue',className:'message__queue',messages})}\nrenderPlayToggleButton(){let buttonTitle='';if(this.state.isStarted){buttonTitle='Reload'}else{buttonTitle='Play'}\nreturn e('div',{key:'page-button',className:'page__play-button animated scaleIn',onClick:()=>this.toggleStarted()},e('p',null,buttonTitle))}\nrenderTime(){const{currentDateTime}=this.state;const datetime=(currentDateTime.getMonth()+1)+\"/\"+currentDateTime.getDate()+\"/\"+currentDateTime.getFullYear()+\" @ \"+currentDateTime.getHours()+\":\"+currentDateTime.getMinutes()+\":\"+currentDateTime.getSeconds();return e('div',{key:'clock',className:'clock animated scaleIn'},e('p',{className:'clock__text'},`${datetime}`))}\nupdateCurrentTime(){this.setState({currentDateTime:new Date()})}\nrender(){return e('div',{key:'page',className:'page'},[this.renderPlayToggleButton(),this.renderMessageQueue(),this.renderTable(),])}};ReactDOM.render(e(Page),document.getElementById('root'))";
                res.Body = pageJS;
                res.AddHeader("Content-Type", "text/javascript");
                return res;
            }

            Response getNewGameController(Request req, Response res, string contextData)
            {
                res.AddHeader("Content-Type", "application/json");
                res.Body = "{ response: new }";
                return res;
            }

            Router router = new Router();
            router.Use("GET", "/", testController);
            router.Use("GET", "/index.html", getIndexHtmlController);
            router.Use("GET", "/index.css", getIndexCssController);
            router.Use("GET", "/index.js", getIndexJsController);
            router.Use("POST", "/random-move", randomMoveController);
            router.Use("GET", "/new-game", getNewGameController);
            router.Use("GET", "/index2.html", getIndex2HtmlController);
            router.Use("GET", "/index2.css", getIndex2CssController);
            router.Use("GET", "/index2.js", getIndex2JsController);

            int port = 5000;

            Server server = new Server(router, false);
            void ListenFinisher(Dictionary<string, object> payload)
            {
                Console.WriteLine($"Listening on port {port}...");
            }

            server.On("listen", ListenFinisher);

            server.ListenWithTCPListener(port);
        }
    }
}
