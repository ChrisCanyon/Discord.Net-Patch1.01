from __future__ import unicode_literals
import youtube_dl
import cv2
import os
from pydub import AudioSegment
import pytesseract
import os
from moviepy.editor import *


class DownloadFormatOptions:
    MP3 = {
    'format': 'bestaudio/best',
    'postprocessors': [{
        'key': 'FFmpegExtractAudio',
        'preferredcodec': 'mp4',
        'preferredquality': '192',
        }],
    }
    MP4_1080p = {
    'format': '137',
    }
    MP4_720p = {
    'format': '136',
    }
    MP4_480p = {
    'format': '135',
    }
    MP4_260p = {
    'format': '134',
    }
    MP4_240p = {
    'format': '133',
    }

def Download(filename, fileFormat):
    with youtube_dl.YoutubeDL(fileFormat) as ydl:
        ydl.download([filename])


def GetImageText(image):
    # Convert the image to gray scale
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    cv2.imshow('unchanged image',gray)
    cv2.waitKey(0) 

    # Performing OTSU threshold
    ret, thresh1 = cv2.threshold(gray, 0, 255, cv2.THRESH_OTSU | cv2.THRESH_BINARY_INV)

    # Specify structure shape and kernel size.
    # Kernel size increases or decreases the area
    # of the rectangle to be detected.
    # A smaller value like (10, 10) will detect
    # each word instead of a sentence.
    rect_kernel = cv2.getStructuringElement(cv2.MORPH_RECT, (20, 20))
    
     # Applying dilation on the threshold image
    dilation = cv2.dilate(thresh1, rect_kernel, iterations = 1)
 
    # Finding contours
    contours, hierarchy = cv2.findContours(dilation, cv2.RETR_EXTERNAL,
                                                     cv2.CHAIN_APPROX_NONE)

    # Apply OCR on the image
    possible = []
    for i in range(1,42):
        possible.append(str(i))
    
    text = ""
    for cnt in contours:
        x, y, w, h = cv2.boundingRect(cnt)
        # Cropping the text block for giving input to OCR
        cropped = gray[y:y + h, x:x + w]

        cv2.imshow('unchanged image',cropped)
        cv2.waitKey(0) 

        text = pytesseract.image_to_string(cropped, config="--psm 10")

        text = text.split('.')[0]
        asInt = None
        try:
            asInt = int(text)
        except:
            bbb = 123
        if(asInt != None):
            return asInt

    return 0

def CreateMP3FromTimestamps(start, stop, filename):
    fullFile = AudioSegment.from_mp3(AOE2_MP3_FILENAME)
    newMP3 = fullFile[start:stop]
    # writing mp3 file
    newMP3.export(f'{filename}.mp3', format="mp3")

AOE2_MP4_FILENAME = 'Age of Empires II - Taunts-nr0zoGjulZ4_Trim.mp4'
AOE2_MP3_FILENAME = 'Age of Empires II - Taunts-nr0zoGjulZ4_Trim.mp3'
pytesseract.pytesseract.tesseract_cmd = 'C:\\Program Files (x86)\\Tesseract-OCR\\tesseract.exe'

os.chdir('C:\\workspace\\Discord.Net-Cory\\YoutubetoMP3\\MP3Downloads\\')
#Download('https://www.youtube.com/watch?v=nr0zoGjulZ4&t=41s&ab_channel=SamLappage', DownloadFormatOptions.MP4_opts)
#Download('https://www.youtube.com/watch?v=nr0zoGjulZ4&t=41s&ab_channel=SamLappage', DownloadFormatOptions.MP4_opts)

#video = VideoFileClip(AOE2_MP4_FILENAME)
#video.audio.write_audiofile(AOE2_MP3_FILENAME)


#open video
video = cv2.VideoCapture(AOE2_MP4_FILENAME)
#get first frame / initial values
success, currentImage = video.read()
currentFrameNumber = 0
framerate = video.get(cv2.CAP_PROP_FPS)
currentImageText = GetImageText(currentImage)
previousImageText = currentImageText
soundBiteStartFrame = 0

while success:
    currentImageText = GetImageText(currentImage)

    if(currentImageText != previousImageText):
        #We found when the end of the sound bite
        #if this frame is different it ended last frame
        soundBiteEndFrame = currentFrameNumber - 1

        #calculate timestamp using fps & start/stop frames
        startMillisecond = (int) (1000 * (soundBiteStartFrame / framerate))
        stopMillisecond = (int) (1000 * (soundBiteEndFrame / framerate))

        #Create snip
        CreateMP3FromTimestamps(startMillisecond, stopMillisecond, previousImageText)

        #update soundBiteStartFrame
        soundBiteStartFrame = currentFrameNumber
  
    #get next frame
    success,currentImage = video.read()
    if(success):
        currentFrameNumber += 1
        previousImageText = currentImageText

#We found when the end of the sound bite
#if this frame is different it ended last frame
soundBiteEndFrame = currentFrameNumber - 1

#calculate timestamp using fps & start/stop frames
startMillisecond = (int) (1000 * (soundBiteStartFrame / framerate))
stopMillisecond = (int) (1000 * (soundBiteEndFrame / framerate))

#Create snip
CreateMP3FromTimestamps(startMillisecond, stopMillisecond, previousImageText)

#update soundBiteStartFrame
soundBiteStartFrame = currentFrameNumber


