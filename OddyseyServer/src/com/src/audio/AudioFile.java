package com.src.audio;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

//Estas son las canciones
@XmlRootElement(name = "Audio")
public class AudioFile {

	@XmlElement(name = "Name")
	public String name;
	@XmlElement(name = "Author")
	public String author;
	@XmlElement(name = "Album")
	public String album;
	@XmlElement(name = "Year")
	public String year;
	@XmlElement(name = "Lyrics")
	public String lyrics;
	@XmlElement(name = "Time")
	public String time;
	@XmlElement(name = "Score")
	public String score;
	@XmlElement(name = "Data")
	public String data;

	@SuppressWarnings("unused")
	private AudioFile() {
		
	}
	
	// Constructor
	public AudioFile(String name, String author, String album, String year, String lyrics, String time) {
		this.name = name;
		this.author = author;
		this.album = album;
		this.year = year;
		this.lyrics = lyrics;
		this.time = time;
	}
	
	//For debugging
	@Override
	public String toString() {
		return name + "/" + author + "/" + album + "/" + year + "/" + lyrics + "/" + time + "/" + score;
	}
	
	
}