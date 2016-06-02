from __future__ import print_function
import httplib2
import os
import base64
import email
import pprint
import re
import pymssql
from apiclient import discovery
import oauth2client
from oauth2client import client
from oauth2client import tools
from apiclient import errors
#os.environ['TDSDUMP'] = 'stdout'

SCOPES = 'https://mail.google.com/'
CLIENT_SECRET_FILE = '/Users/edwardk/go/workspace/src/github.com/gosu517/datacollector/client_secret.json'
APPLICATION_NAME = 'Gmail API Python Quickstart'
conn = pymssql.connect(server="streamanalyticsproject.database.windows.net", port=1433, user="css553user@streamanalyticsproject.database.windows.net", password="CSS553password", database="StreamAnalyticsProject")

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
            #print(message['id'])
            mymessage = service.users().messages().get(userId='me', id=message['id'], format='raw', metadataHeaders=None).execute()
            msg_str = base64.urlsafe_b64decode(mymessage['raw'].encode('ASCII'))

            content = re.search('<div dir="ltr">(.+?)</div>', msg_str)
            if content:
              content=content.group(1)

            author = re.search('From:(.+?)\n', msg_str)
            if author:
              author = author.group(1)

            tstamp = re.search('Date:(.+?)\n', msg_str)
            if tstamp:
              tstamp = tstamp.group(1)

            source='gmail'

            #For demo purposes
            print(author)
            print(content)
            print(tstamp+'\n')

            #Label the message
            service.users().messages().modify(userId='me', id=message['id'], body={"addLabelIds": ["TRASH"]}).execute()


            #---------------iterate through each mail 'id' and post to database then delete ore label
            #cursor = conn.cursor()

            #cursor.execute('SELECT TOP 1 * FROM RawDataTable)
            #row = cursor.fetchone()
            #while row:
        #        print("Id=%d, Author=%s" % (row[0], row[1]))
        #        row = cursor.fetchone()


            #TESTS CONNECTION --------------------------
            #cursor.execute("SELECT @@VERSION")
            #print(cursor.fetchone()[0])


            #cursor.executemany(
            #"INSERT INTO RawDataTable VALUES ('source', 'author', 'timestamp', 'content')",
            #[("gmail", "Edward Kim", "2016-05-16 01:53:19:000", "test message"),
            # ("gmail", "Jane Doe", "2016-05-16 01:53:19:000", "foobar")
            # ])

            #cursor.execute(
            #"INSERT INTO RawDataTable VALUES ('source', 'author', 'timestamp', 'content')",
            #("gmail", author, tstamp, content)
            #)

            #cursor.execute('SELECT * FROM RawDataTable')
            #print(cursor.fetchone())

            #cursor.execute("INSERT StreamAnalyticsProject.RawDataTable ('source', 'author', 'timestamp', 'content') OUTPUT INSERTED.RawDataTableId VALUES ('gmail','Edward Kim','12-14-2015','test message')")

            #row = cursor.fetchone()
            #while row:
            #  print (row[0])
            #  row = cursor.fetchone()

            #conn.commit()


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
#conn.close()

if __name__ == '__main__':
    main()
