window.onload = function(){
	
	var isplaying = true;
	
	var oMusicControl = document.getElementById("musicControl");
	
	oMusicControl.onclick = function(){
		
		var oPlayer = document.getElementById("musicPlayer")
		
		if(isplaying){
			oPlayer.pause();
			this.src = "../../files/imgs/musicBtnOff.png";
		}
		else{
			oPlayer.play();
			this.src = "../../files/imgs/musicBtn.png";
		}
		isplaying= !isplaying;
	}
}
