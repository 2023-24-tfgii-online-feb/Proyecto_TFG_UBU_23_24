import time
import gc
import ujson
import urequests


class ubot:
    
    def __init__(self, token, offset=0):
        self.url = 'https://api.telegram.org/bot' + token
        self.commands = {}
        self.default_handler = None
        self.message_offset = offset
        self.sleep_btw_updates = 3

        messages = self.read_messages()
        if messages:
            if self.message_offset==0:
                self.message_offset = messages[-1]['update_id']
            else:
                for message in messages:
                    if message['update_id'] >= self.message_offset:
                        self.message_offset = message['update_id']
                        break


   def send(self, chat_id, text):
    data = {'chat_id': chat_id, 'text': text}
    try:
        headers = {'Content-type': 'application/json', 'Accept': 'text/plain'}
        response = urequests.post(self.url + '/sendMessage', json=data, headers=headers)
        response.close()
        return True
    except OSError as e:
        print("Error de conexión:", e)
        return False
    except Exception as e:
        print("Error al enviar mensaje:", e)
        return False
