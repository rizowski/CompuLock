class AccountProcess < ActiveRecord::Base
    attr_accessible :account_id, :lastrun, :name

	validates :account_id, presence: true
    validates :name, presence: true
    validates :name, presence: true
    
    belongs_to :account
end
