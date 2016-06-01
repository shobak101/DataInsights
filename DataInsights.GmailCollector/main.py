from __future__ import print_function
import httplib2
import os
import base64
import email
import pprint
import pymssql
from apiclient import discovery
import oauth2client
from oauth2client import client
from oauth2client import tools

SCOPES = 'https://www.googleapis.com/auth/gmail.readonly'
CLIENT_SECRET_FILE = '/Users/edwardk/go/workspace/src/github.com/gosu517/datacollector/client_secret.json'
APPLICATION_NAME = 'Gmail API Python Quickstart'
#conn = pymssql.connect(server='streamanalyticsproject.database.windows.net', user='css553user', password='CSS553password', database='StreamAnalyticsProject')
pp = pprint.PrettyPrinter(indent=2)
def get_credentials():
    """Gets valid user credentials from storage.

    If nothing has been stored, or if the stored credentials are invalid,
    the OAuth2 flow is completed to obtain the new credentials.

    Returns:
        Credentials, the obtained credential.
    """
    home_dir = os.path.expanduser('~')
    credential_dir = os.path.join(home_dir, '.credentials')
    if not os.path.exists(credential_dir):
        os.makedirs(credential_dir)
    credential_path = os.path.join(credential_dir,
                                   'gmail-python-quickstart.json')

    store = oauth2client.file.Storage(credential_path)
    credentials = store.get()
    if not credentials or credentials.invalid:
        flow = client.flow_from_clientsecrets(CLIENT_SECRET_FILE, SCOPES)
        flow.user_agent = APPLICATION_NAME
        if flags:
            credentials = tools.run_flow(flow, store, flags)
        else: # Needed only for compatibility with Python 2.6
            credentials = tools.run(flow, store)
        print('Storing credentials to ' + credential_path)
    return credentials


def main():
    """Shows basic usage of the Gmail API.

    Creates a Gmail API service object and outputs a list of label names
    of the user's Gmail account.
    """
    credentials = get_credentials()
    http = credentials.authorize(httplib2.Http())
    service = discovery.build('gmail', 'v1', http=http)

    #Get list of new mail
    results = service.users().messages().list(userId='me', labelIds=None, q=None, pageToken=None, maxResults=None, includeSpamTrash=None).execute()
    messages = results.get('messages', [])

    if not messages:
        print('No Mail')
    else:
        for message in messages:
            print(message['id'])
            mymessage = service.users().messages().get(userId='me', id=message['id'], format='full', metadataHeaders=None).execute()
            payload = mymessage.get('payload', [])
            parts = payload.get('parts', [])
            pp.pprint(parts)
            #msg_str = base64.urlsafe_b64decode(payload['headers'].encode('ASCII'))
            #print(msg_str)
            #---------------iterate through each mail 'id' and post to database then delete ore label
            #cursor = conn.cursor()
            #cursor.execute("INSERT SalesLT.Product (Name, ProductNumber, StandardCost, ListPrice, SellStartDate) OUTPUT INSERTED.ProductID VALUES ('SQL Server Express', 'SQLEXPRESS', 0, 0, CURRENT_TIMESTAMP)")
            #row = cursor.fetchone()
            #while row:
              #print "Inserted Product ID : " +str(row[0])
              #row = cursor.fetchone()


#msg_str = base64.urlsafe_b64decode(message['raw'].encode('ASCII'))

    #results = service.users().messages().get(userId='me', id='1550535038e335a1', format=None, metadataHeaders=None).execute()
    #print(results)
#    results = service.users().labels().list(userId='me').execute()
#    labels = results.get('labels', [])

#    if not labels:
#        print('No labels found.')
#    else:
#      print('Labels:')
#      for label in labels:
#        print(label['name'])


if __name__ == '__main__':
    main()
